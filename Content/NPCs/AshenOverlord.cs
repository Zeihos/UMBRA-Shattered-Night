using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Projectiles;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.NPCs;
public class AshenOverlord : ModNPC
{
    public override void SetDefaults()
    {
        NPC.width = 98; NPC.height = 104; NPC.lifeMax = 12500;
        NPC.damage = 92; NPC.defense = 30; NPC.knockBackResist = 0f;
        NPC.boss = true; NPC.npcSlots = 12f; NPC.noGravity = true;
        NPC.noTileCollide = true; NPC.aiStyle = -1;
        NPC.value = Item.buyPrice(gold: 17);
        NPC.HitSound = SoundID.NPCHit5; NPC.DeathSound = SoundID.NPCDeath6;
        Music = MusicID.Boss2;
    }
    public override void AI()
    {
        if (NPC.target < 0 || NPC.target >= Main.maxPlayers || !Main.player[NPC.target].active || Main.player[NPC.target].dead)
            NPC.TargetClosest(false);
        if (NPC.target < 0 || NPC.target >= Main.maxPlayers) {
            NPC.EncourageDespawn(10);
            return;
        }
        Player player = Main.player[NPC.target];
        if (!player.active || player.dead || Main.dayTime)
        {
            NPC.velocity.Y -= 0.25f;
            NPC.EncourageDespawn(10);
            return;
        }
        NPC.ai[0]++;
        int phase = NPC.life <= NPC.lifeMax * 0.30f ? 3 : NPC.life <= NPC.lifeMax * 0.65f ? 2 : 1;
        float speed = phase == 3 ? 10.5f : phase == 2 ? 7.5f : 5.5f;
        Vector2 hover = player.Center + new Vector2((float)System.Math.Sin(NPC.ai[0] * 0.025f) * 260f, -175f);
        if (NPC.ai[1] > 0f) { NPC.ai[1]--; NPC.velocity *= 0.99f; }
        else NPC.velocity = Vector2.Lerp(NPC.velocity, (hover - NPC.Center).SafeNormalize(Vector2.UnitY) * speed, 0.055f);
        NPC.rotation = NPC.velocity.X * 0.009f;
        Lighting.AddLight(NPC.Center, 0.9f, 0.35f, 0.08f);
        // ai[2] is a network-synchronized telegraph countdown.
        if (NPC.ai[2] > 0f)
        {
            NPC.ai[2]--;
            NPC.velocity *= 0.94f;
            if (!Main.dedServ && Main.rand.NextBool(2))
            {
                int index = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
                Main.dust[index].noGravity = true;
            }
            if (NPC.ai[2] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int count = phase == 3 ? 16 : 10;
                for (int i = 0; i < count; i++)
                {
                    Vector2 direction = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / count + NPC.ai[0] * 0.012f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 8f,
                        ModContent.ProjectileType<ColossusCinder>(), phase == 3 ? 36 : 28, 1f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }
            return;
        }
        if (Main.netMode == NetmodeID.MultiplayerClient) return;
        int time = (int)NPC.ai[0];
        if (phase >= 2 && time % (phase == 3 ? 220 : 300) == 0)
        {
            NPC.ai[2] = 55f;
            NPC.netUpdate = true;
            return;
        }
        if (time % (phase == 3 ? 105 : 155) == 0)
        {
            NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * (speed + 4f);
            NPC.ai[1] = 22f;
            NPC.netUpdate = true;
        }
        if (time % (phase == 3 ? 40 : phase == 2 ? 65 : 95) == 0)
        {
            int shots = phase == 3 ? 5 : phase == 2 ? 3 : 1;
            Vector2 aim = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            for (int i = 0; i < shots; i++)
            {
                Vector2 velocity = aim.RotatedBy(MathHelper.ToRadians((i - (shots - 1) / 2f) * 13f)) * 9f;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<ColossusCinder>(), phase == 3 ? 34 : 26, 1f, Main.myPlayer);
            }
        }
    }
    public override void ModifyNPCLoot(NPCLoot loot)
    {
        loot.Add(ItemDropRule.Common(ModContent.ItemType<OverlordHeart>(), 1, 5, 9));
    }
    public override void OnKill()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) return;
        UmbraWorld.DownedAshenOverlord = true;
        if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.WorldData);
    }
}
