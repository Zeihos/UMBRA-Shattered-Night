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

// Post-invasion boss. Projectile creation is server-authoritative.
public class RiftArchon : ModNPC
{
    public override void SetStaticDefaults() { Main.npcFrameCount[Type] = 6; }
    
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

    public override void SetDefaults()
    {
        NPC.width = 82; NPC.height = 82; NPC.lifeMax = 4200;
        NPC.damage = 48; NPC.defense = 16; NPC.knockBackResist = 0f;
        NPC.noGravity = true; NPC.noTileCollide = true; NPC.boss = true;
        NPC.npcSlots = 10f; NPC.aiStyle = -1;
        NPC.value = Item.buyPrice(gold: 7);
        NPC.HitSound = SoundID.NPCHit3; NPC.DeathSound = SoundID.NPCDeath6;
        Music = MusicID.Boss2;
    }
    public override void AI()
    {
        NPC.TargetClosest(false);
        if (NPC.target < 0 || NPC.target >= Main.maxPlayers)
        {
            NPC.EncourageDespawn(10);
            return;
        }

        Player target = Main.player[NPC.target];
        if (!target.active || target.dead || Main.dayTime)
        {
            NPC.velocity.Y -= 0.25f;
            NPC.EncourageDespawn(12);
            return;
        }

        NPC.ai[0]++;
        float timer = NPC.ai[0];
        bool phaseTwo = NPC.life < NPC.lifeMax * 0.60f;
        bool phaseThree = NPC.life < NPC.lifeMax * 0.25f;
        float speed = phaseThree ? 10f : phaseTwo ? 8f : 6f;

        // ai[1]: dash duration, ai[2]: radial warning, ai[3]: dash windup.
        // The three attack states are mutually exclusive; countdown values
        // are networked while the authority alone launches projectiles.
        NPC.rotation = NPC.velocity.X * 0.016f;
        Lighting.AddLight(NPC.Center, 0.65f, 0.12f, 0.85f);

        if (NPC.ai[2] > 0f)
        {
            NPC.ai[2]--;
            NPC.velocity *= 0.92f;
            if (!Main.dedServ && Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PurpleTorch);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.2f;
            }

            if (NPC.ai[2] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int count = phaseThree ? 12 : 8;
                for (int j = 0; j < count; j++)
                {
                    float angle = MathHelper.TwoPi * j / count + timer * 0.012f;
                    Vector2 direction = angle.ToRotationVector2();
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center,
                        direction * (phaseThree ? 9f : 7f),
                        ModContent.ProjectileType<UmbralBolt>(), phaseThree ? 30 : 24,
                        1f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }
            return;
        }

        if (NPC.ai[3] > 0f)
        {
            NPC.ai[3]--;
            NPC.velocity *= 0.85f;
            if (!Main.dedServ && Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PurpleTorch);
                Main.dust[d].noGravity = true;
            }
            if (NPC.ai[3] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY) *
                    (phaseThree ? 17f : phaseTwo ? 14f : 11f);
                NPC.ai[1] = 20f;
                NPC.netUpdate = true;
            }
            return;
        }

        if (NPC.ai[1] > 0f)
        {
            NPC.ai[1]--;
            NPC.velocity *= 0.992f;
            if (NPC.ai[1] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
                NPC.netUpdate = true;
            return;
        }

        Vector2 hover = target.Center +
            new Vector2((float)System.Math.Sin(timer * 0.022f) * 245f, -190f);
        NPC.velocity = Vector2.Lerp(NPC.velocity,
            (hover - NPC.Center).SafeNormalize(Vector2.UnitY) * speed, 0.045f);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        if (phaseTwo && (int)timer % (phaseThree ? 240 : 340) == 0)
        {
            NPC.ai[2] = 45f;
            NPC.netUpdate = true;
            return;
        }

        if ((int)timer % (phaseThree ? 120 : phaseTwo ? 160 : 220) == 65)
        {
            NPC.ai[3] = 24f;
            NPC.netUpdate = true;
            return;
        }

        int shootInterval = phaseThree ? 48 : phaseTwo ? 70 : 105;
        if ((int)timer % shootInterval == 0)
        {
            int count = phaseThree ? 5 : phaseTwo ? 3 : 1;
            Vector2 aim = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            for (int i = 0; i < count; i++)
            {
                Vector2 velocity = aim.RotatedBy((i - (count - 1) / 2f) * 0.18f) *
                    (phaseThree ? 11f : phaseTwo ? 9f : 7f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<UmbralBolt>(),
                    phaseThree ? 32 : phaseTwo ? 27 : 22, 1f, Main.myPlayer);
            }
        }
    }
    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<ArchonHeart>(), 1, 3, 5));
        loot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(), 1, 20, 35));
    }
    public override void OnKill()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) return;
        UmbraWorld.DownedArchon = true;
        if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.WorldData);
    }
}
