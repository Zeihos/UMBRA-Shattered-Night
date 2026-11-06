using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.NPCs;
public class RiftMite : ModNPC {
 public override void SetDefaults() {
 NPC.width=28; NPC.height=24; NPC.damage=21; NPC.defense=5; NPC.lifeMax=75;
 NPC.value=Item.buyPrice(silver:4); NPC.knockBackResist=.65f;
 NPC.aiStyle=1; AIType=NPCID.BlueSlime;
 NPC.HitSound=SoundID.NPCHit1; NPC.DeathSound=SoundID.NPCDeath1;
 }
 public override float SpawnChance(NPCSpawnInfo spawnInfo) =>
 spawnInfo.Player.InModBiome<FracturedAbyss>() && !spawnInfo.PlayerSafe ? .36f : 0f;
 public override void ModifyNPCLoot(NPCLoot npcLoot) {
 npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(),3,1,2));
 npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<UmbralOreItem>(),5,1,3));
 }
 public override void AI() { Lighting.AddLight(NPC.Center,.14f,.02f,.23f); }
}
