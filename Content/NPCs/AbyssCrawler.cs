using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.NPCs;
public class AbyssCrawler : ModNPC {
 public override void SetDefaults() {
  NPC.width=30; NPC.height=20; NPC.damage=27; NPC.defense=9; NPC.lifeMax=110;
  NPC.value=Item.buyPrice(silver:6); NPC.knockBackResist=.4f;
  NPC.aiStyle=3; AIType=NPCID.AngryBones;
 }
 public override float SpawnChance(NPCSpawnInfo spawnInfo) =>
  spawnInfo.Player.InModBiome<FracturedAbyss>() && !spawnInfo.PlayerSafe ? 0.28f : 0f;
 public override void ModifyNPCLoot(NPCLoot npcLoot) {
  npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(),2,1,3));
  npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<UmbralOreItem>(),4,1,2));
 }
 public override void AI() { Lighting.AddLight(NPC.Center,.18f,.03f,.26f); }
}
