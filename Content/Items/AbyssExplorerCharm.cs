using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
namespace UmbraShatteredNight.Content.Items;
public class AbyssExplorerCharm : ModItem {
 public override void SetDefaults() {
  Item.width=26; Item.height=28; Item.accessory=true;
  Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:1);
 }
 public override void UpdateAccessory(Player player, bool hideVisual) {
  if(player.InModBiome<FracturedAbyss>()) {
   player.moveSpeed += 0.12f;
   player.statDefense += 3;
   if(!hideVisual) Lighting.AddLight(player.Center,0.15f,0.07f,0.28f);
  }
 }
 public override void AddRecipes() => CreateRecipe()
  .AddIngredient<UmbralBar>(8).AddIngredient<VoidShard>(10)
  .AddTile(TileID.Anvils).Register();
}
