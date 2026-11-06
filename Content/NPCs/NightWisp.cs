using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.NPCs;
public class NightWisp : ModNPC
{
    public override void SetDefaults() {
        NPC.width = 30; NPC.height = 32;
        NPC.damage = 22; NPC.defense = 6;
        NPC.lifeMax = 75; NPC.knockBackResist = 0.6f;
        NPC.value = 250f;
        NPC.aiStyle = 2;
        AIType = NPCID.DemonEye;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo) {
        if (Main.dayTime || !spawnInfo.Player.ZoneOverworldHeight || spawnInfo.PlayerSafe) return 0f;
        return 0.11f;
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot) {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(), 1, 1, 3));
    }
    public override void FindFrame(int frameHeight) {
        NPC.frameCounter += 0.15;
        if (NPC.frameCounter >= 4) NPC.frameCounter = 0;
        NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
    }
    public override void AI() {
        NPC.rotation = NPC.velocity.X * 0.06f;
        Lighting.AddLight(NPC.Center, 0.24f, 0.10f, 0.42f);
    }
}
