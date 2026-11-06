using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
// Crafted only after obtaining loot from both the Sentinel and the Devourer.
public class ConvergenceSigil : ModItem
{
    public override void SetDefaults() {
        Item.width = 26; Item.height = 30; Item.maxStack = 9999;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 1);
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<EclipseCore>(3)
        .AddIngredient<DevourerFang>(5)
        .AddIngredient<UmbralBar>(10)
        .AddTile(TileID.Anvils).Register();
}
