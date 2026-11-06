using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
namespace UmbraShatteredNight.Content.Items;
public class ColossusWard : ModItem {
 public override void SetDefaults() {
  Item.width=26; Item.height=28; Item.accessory=true;
  Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:6);
 }
 public override void UpdateAccessory(Player player, bool hideVisual) {
  player.endurance += 0.04f;
  if (player.InModBiome<AshenWastes>()) player.statDefense+=6;
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<AshenCore>(6).AddIngredient<CinderBar>(12).AddTile(TileID.MythrilAnvil).Register();
}
