using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Systems;
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
  if(NPC.target<0 || NPC.target>=Main.maxPlayers || !Main.player[NPC.target].active || Main.player[NPC.target].dead) NPC.TargetClosest(false);
  if(NPC.target<0 || NPC.target>=Main.maxPlayers || !Main.player[NPC.target].active || Main.player[NPC.target].dead) {NPC.EncourageDespawn(10);return;}
  Player player=Main.player[NPC.target];
  NPC.ai[0]++;
  bool enraged=NPC.life < NPC.lifeMax/2;
  Vector2 delta=player.Center+new Vector2(0f,-110f)-NPC.Center;
  if(delta.LengthSquared()>1f) {delta.Normalize(); NPC.velocity=Vector2.Lerp(NPC.velocity,delta*(enraged?7f:4f),0.06f);}
  Lighting.AddLight(NPC.Center,0.7f,0.2f,0.04f);
  int interval=enraged?70:105;
  if(Main.netMode!=NetmodeID.MultiplayerClient && (int)NPC.ai[0]%interval==0) {
   Vector2 aim=player.Center-NPC.Center;
   if(aim.LengthSquared()<1f)aim=Vector2.UnitY;
   aim.Normalize();
   for(int i=(enraged?-1:0);i<=(enraged?1:0);i++) {
    Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center,aim.RotatedBy(MathHelper.ToRadians(i*17))*7f,
       ProjectileID.Fireball,28,1f,Main.myPlayer);
   }
  }
 }
 public override void ModifyNPCLoot(NPCLoot loot) { loot.Add(ItemDropRule.Common(ModContent.ItemType<AshenCore>(),1,3,5)); }
}
