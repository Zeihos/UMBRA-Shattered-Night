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
 public override void SetStaticDefaults() { Main.npcFrameCount[Type]=6; }
 
    // Six hand-finished animation frames. The sprite sheet preserves the original
    // per-frame bounds so collision and attack positioning do not change.
    public override void FindFrame(int frameHeight)
    {
        float pace = NPC.life < NPC.lifeMax * 0.4f ? 0.17f : 0.11f;
        if (NPC.ai[2] > 0f) pace += 0.045f; // make charged attacks more legible
        NPC.frameCounter += pace;
        if (NPC.frameCounter >= 6d) NPC.frameCounter -= 6d;
        NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
    }

    public override void SetDefaults() {
  NPC.width=54; NPC.height=56; NPC.lifeMax=2200; NPC.damage=72; NPC.defense=24;
  NPC.noGravity=true; NPC.noTileCollide=true; NPC.knockBackResist=0f;
  NPC.aiStyle=-1; NPC.value=Item.buyPrice(gold:4);
  NPC.HitSound=SoundID.NPCHit3; NPC.DeathSound=SoundID.NPCDeath6;
 }
 public override float SpawnChance(NPCSpawnInfo info) =>
  UmbraWorld.DownedAstralSovereign && !info.PlayerSafe && info.Player.InModBiome<AshenWastes>() && !NPC.AnyNPCs(Type) ? 0.003f : 0f;
    public override void AI()
    {
        if (!UMBRAEnemyTargeting.TryGetTarget(NPC, out Player player, 2000f))
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.EncourageDespawn(10);
            return;
        }

        NPC.ai[0]++;
        bool enraged = NPC.life <= NPC.lifeMax / 2;
        int cycle = enraged ? 210 : 270;
        int tick = (int)NPC.ai[0] % cycle;
        // ai[1] holds an active dash, ai[2] the 45-tick radial warning.
        // Timers are shared by the server and clients; only the authority
        // can start a charge or create hostile projectiles.
        Lighting.AddLight(NPC.Center, NPC.ai[2] > 0f ? 1f : 0.7f,
            NPC.ai[2] > 0f ? 0.42f : 0.2f, 0.04f);

        if (NPC.ai[2] > 0f)
        {
            NPC.ai[2]--;
            NPC.velocity *= 0.88f;
            if (!Main.dedServ && Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
                Main.dust[dust].noGravity = true;
            }

            if (NPC.ai[2] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int count = enraged ? 12 : 8;
                for (int i = 0; i < count; i++)
                {
                    Vector2 velocity = Vector2.UnitX.RotatedBy(
                        MathHelper.TwoPi * i / count + NPC.ai[0] * 0.02f) *
                        (enraged ? 8f : 6.5f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center,
                        velocity, ModContent.ProjectileType<ColossusCinder>(),
                        32, 1f, Main.myPlayer);
                }

                NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) *
                    (enraged ? 14f : 11f);
                NPC.ai[1] = 17f;
                NPC.netUpdate = true;
            }
            return;
        }

        if (NPC.ai[1] > 0f)
        {
            NPC.ai[1]--;
            NPC.velocity *= 0.985f;
            if (NPC.ai[1] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
                NPC.netUpdate = true;
            return;
        }

        Vector2 destination = player.Center + new Vector2(0f, -125f);
        Vector2 directionToHover = (destination - NPC.Center).SafeNormalize(Vector2.UnitY);
        NPC.velocity = Vector2.Lerp(NPC.velocity,
            directionToHover * (enraged ? 7f : 4.4f), 0.06f);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        // The phase may change mid-fight. An explicit charge countdown
        // ensures a 45-tick warning regardless of the active cycle length.
        if (tick == cycle - 65)
        {
            NPC.ai[2] = 45f;
            NPC.netUpdate = true;
            return;
        }

        if (tick > 0 && tick < cycle - 65 && tick % (enraged ? 65 : 90) == 0)
        {
            Vector2 aim = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            int count = enraged ? 3 : 1;
            for (int i = 0; i < count; i++)
            {
                float angle = (i - (count - 1) / 2f) * MathHelper.ToRadians(14);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center,
                    aim.RotatedBy(angle) * (enraged ? 9f : 7f),
                    ModContent.ProjectileType<ColossusCinder>(), 28, 1f, Main.myPlayer);
            }
        }
    }
public override void OnKill() {
  if (Main.netMode == NetmodeID.MultiplayerClient) return;
  UmbraWorld.DownedAshenColossus = true;
  if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.WorldData);
 }
 public override void ModifyNPCLoot(NPCLoot loot) { loot.Add(ItemDropRule.Common(ModContent.ItemType<AshenCore>(),1,4,6)); }
}
