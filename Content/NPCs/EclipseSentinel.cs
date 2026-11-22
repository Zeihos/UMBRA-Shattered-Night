using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Projectiles;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.NPCs;
public class EclipseSentinel : ModNPC
{
    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 6;

    
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
        NPC.width = 76; NPC.height = 76;
        NPC.damage = 34; NPC.defense = 8; NPC.lifeMax = 1800;
        NPC.knockBackResist = 0f; NPC.value = Item.buyPrice(gold: 3);
        NPC.boss = true; NPC.noGravity = true; NPC.noTileCollide = true;
        NPC.npcSlots = 10f; NPC.aiStyle = -1;
        NPC.HitSound = SoundID.NPCHit3; NPC.DeathSound = SoundID.NPCDeath6;
        Music = MusicID.Boss1;
    }
    public override void AI()
    {
        NPC.TargetClosest(false);
        if (NPC.target < 0 || NPC.target >= Main.maxPlayers)
        {
            NPC.EncourageDespawn(10);
            return;
        }

        Player player = Main.player[NPC.target];
        if (!player.active || player.dead || Main.dayTime)
        {
            NPC.velocity.Y -= 0.22f;
            NPC.EncourageDespawn(15);
            return;
        }

        // ai[0]: attack cycle; ai[1]: active dash; ai[2]: dash warning.
        // All three are synchronized by tModLoader. Projectiles are created
        // only by the SP host or server; remote clients render the warning.
        NPC.ai[0]++;
        bool phaseTwo = NPC.life < NPC.lifeMax / 2;
        float timer = NPC.ai[0];
        float hoverSpeed = phaseTwo ? 9f : 6.5f;
        NPC.rotation = NPC.velocity.X * 0.025f;
        Lighting.AddLight(NPC.Center, NPC.ai[2] > 0f ? 0.75f : 0.5f, 0.1f, 0.7f);

        if (NPC.ai[2] > 0f)
        {
            // Slow down and visibly charge before the dash.
            NPC.ai[2]--;
            NPC.velocity *= 0.86f;
            if (!Main.dedServ && Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PurpleTorch);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.28f;
            }
            if (NPC.ai[2] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) *
                    (phaseTwo ? 13f : 10f);
                NPC.ai[1] = 18f;
                NPC.netUpdate = true;
            }
            return;
        }

        if (NPC.ai[1] > 0f)
        {
            // Preserve the dash velocity for multiple ticks instead of
            // immediately blending it back into the hovering movement.
            NPC.ai[1]--;
            NPC.velocity *= 0.992f;
            if (NPC.ai[1] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
                NPC.netUpdate = true;
            return;
        }

        Vector2 hoverTarget = player.Center +
            new Vector2((float)System.Math.Sin(timer * 0.025f) * 190f, -150f);
        NPC.velocity = Vector2.Lerp(NPC.velocity,
            (hoverTarget - NPC.Center).SafeNormalize(Vector2.UnitY) * hoverSpeed, 0.045f);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        int dashInterval = phaseTwo ? 95 : 145;
        if ((int)timer % dashInterval == 0)
        {
            NPC.ai[2] = 30f;
            NPC.netUpdate = true;
            return;
        }

        int fireInterval = phaseTwo ? 70 : 110;
        if ((int)timer % fireInterval == 35)
        {
            Vector2 aim = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            int shots = phaseTwo ? 3 : 1;
            for (int i = 0; i < shots; i++)
            {
                Vector2 velocity = aim.RotatedBy((i - (shots - 1) / 2f) * 0.22f) *
                    (phaseTwo ? 9f : 7f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<UmbralBolt>(), phaseTwo ? 24 : 18,
                    1f, Main.myPlayer);
            }
        }
    }
    public override void OnKill() {
        if (Main.netMode == NetmodeID.MultiplayerClient) return;
        UmbraWorld.DownedSentinel = true;
        if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.WorldData);
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot) {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(), 1, 20, 35));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EclipseCore>(), 1, 4, 6));
    }
}
