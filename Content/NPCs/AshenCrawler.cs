using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.NPCs;
public class AshenCrawler : ModNPC {
 public override void SetDefaults() {
  NPC.width=34; NPC.height=26; NPC.lifeMax=520;
  NPC.damage=67; NPC.defense=22; NPC.knockBackResist=0.3f;
  NPC.value=Item.buyPrice(silver:40);
  NPC.aiStyle=3; AIType=NPCID.Fighter;
  NPC.HitSound=SoundID.NPCHit3; NPC.DeathSound=SoundID.NPCDeath6;
 }
 public override float SpawnChance(NPCSpawnInfo spawnInfo) =>
  UmbraWorld.DownedAstralSovereign && !spawnInfo.PlayerSafe && spawnInfo.Player.InModBiome<AshenWastes>() ? 0.12f : 0f;
 public override void AI() { Lighting.AddLight(NPC.Center, 0.35f, 0.09f, 0.035f); }
 public override void ModifyNPCLoot(NPCLoot npcLoot) {
  npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CinderOreItem>(),2,1,3));
 }
}
