using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
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
        float phaseSpeed = NPC.life < NPC.lifeMax / 2 ? 9f : 6.5f;
        Vector2 target = player.Center + new Vector2((float)System.Math.Sin(NPC.ai[0] * 0.025f) * 190f, -150f);
        Vector2 desired = (target - NPC.Center).SafeNormalize(Vector2.UnitY) * phaseSpeed;
        NPC.velocity = Vector2.Lerp(NPC.velocity, desired, 0.045f);
        NPC.rotation = NPC.velocity.X * 0.025f;
        Lighting.AddLight(NPC.Center, 0.5f, 0.1f, 0.7f);
        int frequency = NPC.life < NPC.lifeMax / 2 ? 95 : 145;
        if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] % frequency == 0) {
            Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
            NPC.velocity = direction * (NPC.life < NPC.lifeMax / 2 ? 13f : 10f);
            NPC.netUpdate = true;
        }
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot) {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(), 1, 20, 35));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EclipseCore>(), 1, 3, 6));
    }
}
