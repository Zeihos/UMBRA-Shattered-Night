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
    public override void SetStaticDefaults() { Main.npcFrameCount[Type] = 1; }
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
        Vector2 hover = target.Center + new Vector2((float)System.Math.Sin(timer * 0.022f) * 245f, -190f);
        // ai[1] is a synchronized dash duration. Preserve dash speed before returning to hovering.
        if (NPC.ai[1] > 0f)
        {
            NPC.ai[1]--;
            NPC.velocity *= 0.992f;
            if (NPC.ai[1] <= 0f) NPC.netUpdate = true;
        }
        else
        {
            NPC.velocity = Vector2.Lerp(NPC.velocity, (hover - NPC.Center).SafeNormalize(Vector2.UnitY) * speed, 0.045f);
        }
        // Telegraph the radial void burst for 45 ticks in phase 2/3.
        // ai[2] counts down on all peers; only the server creates damaging projectiles.
        if (NPC.ai[2] > 0f)
        {
            NPC.ai[2]--;
            NPC.velocity *= 0.95f;
            if (Main.netMode != NetmodeID.Server && Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PurpleTorch);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.2f;
            }
        }
        NPC.rotation = NPC.velocity.X * 0.016f;
        Lighting.AddLight(NPC.Center, 0.65f, 0.12f, 0.85f);
        if (Main.netMode == NetmodeID.MultiplayerClient) return;
        // At the end of the telegraph, fire a rotating ring of bolts.
        if (NPC.ai[2] == 0f && NPC.localAI[0] == 1f && (phaseTwo || phaseThree))
        {
            int amount = phaseThree ? 12 : 8;
            for (int j = 0; j < amount; j++)
            {
                float angle = MathHelper.TwoPi * j / amount + timer * 0.012f;
                Vector2 direction = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle));
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * (phaseThree ? 9f : 7f),
                    ModContent.ProjectileType<UmbralBolt>(), phaseThree ? 30 : 24, 1f, Main.myPlayer);
            }
            NPC.localAI[0] = 0f;
            NPC.netUpdate = true;
        }
        if (NPC.ai[2] <= 0f && (phaseTwo || phaseThree) && (int)timer % (phaseThree ? 240 : 340) == 0)
        {
            NPC.ai[2] = 45f;
            NPC.localAI[0] = 1f;
            NPC.netUpdate = true;
        }
        int shootInterval = phaseThree ? 48 : phaseTwo ? 70 : 105;
        if (NPC.ai[2] <= 0f && (int)timer % shootInterval == 0)
        {
            int count = phaseThree ? 5 : phaseTwo ? 3 : 1;
            Vector2 aim = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            for (int i = 0; i < count; i++)
            {
                Vector2 velocity = aim.RotatedBy((i - (count - 1) / 2f) * 0.18f) * (phaseThree ? 11f : phaseTwo ? 9f : 7f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<UmbralBolt>(), phaseThree ? 32 : phaseTwo ? 27 : 22, 1f, Main.myPlayer);
            }
        }
        if (NPC.ai[2] <= 0f && (int)timer % (phaseThree ? 120 : phaseTwo ? 160 : 220) == 65)
        {
            NPC.velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * (phaseThree ? 17f : phaseTwo ? 14f : 11f);
            NPC.ai[1] = 20f;
            NPC.netUpdate = true;
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
