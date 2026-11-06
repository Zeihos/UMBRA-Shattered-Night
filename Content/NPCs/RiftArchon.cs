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
        NPC.velocity = Vector2.Lerp(NPC.velocity, (hover - NPC.Center).SafeNormalize(Vector2.UnitY) * speed, 0.045f);
        NPC.rotation = NPC.velocity.X * 0.016f;
        Lighting.AddLight(NPC.Center, 0.65f, 0.12f, 0.85f);
        if (Main.netMode == NetmodeID.MultiplayerClient) return;
        int shootInterval = phaseThree ? 48 : phaseTwo ? 70 : 105;
        if ((int)timer % shootInterval == 0)
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
        if ((int)timer % (phaseThree ? 120 : phaseTwo ? 160 : 220) == 65)
        {
            NPC.velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * (phaseThree ? 17f : phaseTwo ? 14f : 11f);
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
