using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using UmbraShatteredNight.Content.Biomes;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.NPCs;
public class AbyssStalker : ModNPC {
 public override void SetDefaults() {
  NPC.width=32; NPC.height=32; NPC.lifeMax=230; NPC.damage=42; NPC.defense=12;
  NPC.knockBackResist=.25f; NPC.value=Item.buyPrice(silver:18);
  NPC.aiStyle=14; AIType=NPCID.DemonEye; AnimationType=NPCID.DemonEye;
  NPC.noGravity=true; NPC.HitSound=SoundID.NPCHit3; NPC.DeathSound=SoundID.NPCDeath6;
 }
 public override float SpawnChance(NPCSpawnInfo spawnInfo)=>Main.hardMode && spawnInfo.Player.InModBiome<FracturedAbyss>() && !spawnInfo.PlayerSafe ? .085f : 0f;
 public override void AI(){ Lighting.AddLight(NPC.Center,.2f,.02f,.3f); }
 public override void ModifyNPCLoot(NPCLoot loot){ loot.Add(ItemDropRule.Common(ModContent.ItemType<VoidShard>(),1,2,5)); loot.Add(ItemDropRule.Common(ModContent.ItemType<UmbralOreItem>(),2,1,4)); }
}
