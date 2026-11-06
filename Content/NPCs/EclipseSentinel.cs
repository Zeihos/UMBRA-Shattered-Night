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
    public override void SetDefaults() {
        NPC.width = 76; NPC.height = 76;
        NPC.damage = 34; NPC.defense = 8; NPC.lifeMax = 1800;
        NPC.knockBackResist = 0f; NPC.value = Item.buyPrice(gold: 3);
        NPC.boss = true; NPC.noGravity = true; NPC.noTileCollide = true;
        NPC.npcSlots = 10f; NPC.aiStyle = -1;
        NPC.HitSound = SoundID.NPCHit3; NPC.DeathSound = SoundID.NPCDeath6;
        Music = MusicID.Boss1;
    }
    public override void AI() {
        NPC.TargetClosest(false);
        Player player = Main.player[NPC.target];
        if (!player.active || player.dead || Main.dayTime) {
            NPC.velocity.Y -= 0.22f;
            NPC.EncourageDespawn(15);
            return;
        }
        NPC.ai[0]++;
        bool phaseTwo = NPC.life < NPC.lifeMax / 2;
        float timer = NPC.ai[0];
        float phaseSpeed = phaseTwo ? 9f : 6.5f;
        Vector2 target = player.Center + new Vector2((float)System.Math.Sin(timer * 0.025f) * 190f, -150f);
        Vector2 desired = (target - NPC.Center).SafeNormalize(Vector2.UnitY) * phaseSpeed;
        NPC.velocity = Vector2.Lerp(NPC.velocity, desired, 0.045f);
        NPC.rotation = NPC.velocity.X * 0.025f;
        Lighting.AddLight(NPC.Center, 0.5f, 0.1f, 0.7f);
        if (Main.netMode == NetmodeID.MultiplayerClient) return;
        int dashInterval = phaseTwo ? 95 : 145;
        if ((int)timer % dashInterval == 0) {
            NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * (phaseTwo ? 13f : 10f);
            NPC.netUpdate = true;
        }
        int fireInterval = phaseTwo ? 70 : 110;
        if ((int)timer % fireInterval == 35) {
            Vector2 aim = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            int shots = phaseTwo ? 3 : 1;
            for (int i = 0; i < shots; i++) {
                Vector2 velocity = aim.RotatedBy((i - (shots - 1) / 2f) * 0.22f) * (phaseTwo ? 9f : 7f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<UmbralBolt>(), phaseTwo ? 24 : 18, 1f, Main.myPlayer);
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
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EclipseCore>(), 1, 3, 6));
    }
}
