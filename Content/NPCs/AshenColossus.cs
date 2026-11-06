using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Systems;
using UmbraShatteredNight.Content.Projectiles;
namespace UmbraShatteredNight.Content.NPCs;
public class AshenColossus : ModNPC {
 public override void SetStaticDefaults() { Main.npcFrameCount[Type]=1; }
 public override void SetDefaults() {
  NPC.width=54; NPC.height=56; NPC.lifeMax=2200; NPC.damage=72; NPC.defense=24;
  NPC.noGravity=true; NPC.noTileCollide=true; NPC.knockBackResist=0f;
  NPC.aiStyle=-1; NPC.value=Item.buyPrice(gold:4);
  NPC.HitSound=SoundID.NPCHit3; NPC.DeathSound=SoundID.NPCDeath6;
 }
 public override float SpawnChance(NPCSpawnInfo info) =>
  UmbraWorld.DownedAstralSovereign && !info.PlayerSafe && info.Player.InModBiome<AshenWastes>() && !NPC.AnyNPCs(Type) ? 0.003f : 0f;
 public override void AI() {
  if (NPC.target < 0 || NPC.target >= Main.maxPlayers || !Main.player[NPC.target].active || Main.player[NPC.target].dead)
   NPC.TargetClosest(false);
  if (NPC.target < 0 || NPC.target >= Main.maxPlayers || !Main.player[NPC.target].active || Main.player[NPC.target].dead) {
   NPC.EncourageDespawn(10);
   return;
  }
  Player player = Main.player[NPC.target];
  bool enraged = NPC.life <= NPC.lifeMax / 2;
  // ai[0] is the attack cycle, synced by tModLoader. ai[1] holds a short burst-dash timer.
  NPC.ai[0]++;
  int cycle = enraged ? 210 : 270;
  int tick = (int)NPC.ai[0] % cycle;
  int warningStart = cycle - 65;
  int firingTick = cycle - 20;
  bool warning = tick >= warningStart && tick < firingTick;
  if (NPC.ai[1] > 0f) {
   NPC.ai[1]--;
   NPC.velocity *= 0.985f;
  } else {
   Vector2 destination = player.Center + new Vector2(0f, -125f);
   Vector2 direction = destination - NPC.Center;
   if (direction.LengthSquared() > 1f) {
    direction.Normalize();
    NPC.velocity = Vector2.Lerp(NPC.velocity, direction * (warning ? 2f : enraged ? 7f : 4.4f), 0.06f);
   }
  }
  Lighting.AddLight(NPC.Center, warning ? 1f : 0.7f, warning ? 0.42f : 0.2f, 0.04f);
  if (warning && Main.rand.NextBool(3)) {
   int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
   Main.dust[dust].noGravity = true;
  }
  if (Main.netMode == NetmodeID.MultiplayerClient) return;
  if (!warning && tick > 0 && tick < warningStart && tick % (enraged ? 65 : 90) == 0) {
   Vector2 aim = player.Center - NPC.Center;
   if (aim.LengthSquared() < 1f) aim = Vector2.UnitY;
   aim.Normalize();
   int count = enraged ? 3 : 1;
   for (int i = 0; i < count; i++) {
    float angle = (i - (count - 1) / 2f) * MathHelper.ToRadians(14);
    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center,
     aim.RotatedBy(angle) * (enraged ? 9f : 7f),
     ModContent.ProjectileType<ColossusCinder>(), 28, 1f, Main.myPlayer);
   }
  }
  if (tick == firingTick) {
   int count = enraged ? 12 : 8;
   for (int i = 0; i < count; i++) {
    Vector2 velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / count + NPC.ai[0] * 0.02f) * (enraged ? 8f : 6.5f);
    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
     ModContent.ProjectileType<ColossusCinder>(), 32, 1f, Main.myPlayer);
   }
   // Brief dash after the ring. Avoid resetting velocity on the very next tick.
   Vector2 dash = player.Center - NPC.Center;
   if (dash.LengthSquared() > 1f) {
    dash.Normalize(); NPC.velocity = dash * (enraged ? 14f : 11f);
    NPC.ai[1] = 17f;
   }
   NPC.netUpdate = true;
  }
 }
 public override void OnKill() {
  if (Main.netMode == NetmodeID.MultiplayerClient) return;
  UmbraWorld.DownedAshenColossus = true;
  if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.WorldData);
 }
 public override void ModifyNPCLoot(NPCLoot loot) { loot.Add(ItemDropRule.Common(ModContent.ItemType<AshenCore>(),1,3,5)); }
}
