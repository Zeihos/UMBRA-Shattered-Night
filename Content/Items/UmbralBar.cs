using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class UmbralBar : ModItem {
 public override void SetDefaults() { Item.width=20; Item.height=16; Item.maxStack=9999; Item.value=Item.sellPrice(silver:20); Item.rare=ItemRarityID.Blue; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralOreItem>(4).AddTile(TileID.Furnaces).Register();
}
