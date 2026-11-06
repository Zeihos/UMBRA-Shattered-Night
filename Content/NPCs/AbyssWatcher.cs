using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.NPCs;
public class AbyssWatcher : ModNPC {
 public override void SetDefaults() {
  NPC.width=30; NPC.height=38; NPC.lifeMax=160; NPC.damage=31; NPC.defense=9;
  NPC.knockBackResist=.35f; NPC.value=Item.buyPrice(silver:12);
  NPC.aiStyle=22; AIType=NPCID.Wraith; AnimationType=NPCID.Wraith;
  NPC.noGravity=true; NPC.noTileCollide=true;
  NPC.HitSound=SoundID.NPCHit3; NPC.DeathSound=SoundID.NPCDeath6;
 }
 public override float SpawnChance(NPCSpawnInfo spawnInfo) =>
  spawnInfo.Player.InModBiome<FracturedAbyss>() && !spawnInfo.PlayerSafe && Main.hardMode ? .11f : 0f;
 public override void AI() { Lighting.AddLight(NPC.Center,.23f,.05f,.33f); }
 public override void ModifyNPCLoot(NPCLoot loot) {
  loot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(),2,2,5));
  loot.Add(ItemDropRule.Common(ModContent.ItemType<UmbralOreItem>(),3,2,4));
 }
}
