using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class CinderBar : ModItem {
 public override void SetDefaults() {
  Item.width=24; Item.height=16; Item.maxStack=9999;
  Item.value=Item.sellPrice(silver:85); Item.rare=ItemRarityID.Pink;
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<CinderOreItem>(4).AddTile(TileID.AdamantiteForge).Register();
}
