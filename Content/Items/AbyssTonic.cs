using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class AbyssTonic : ModItem {
 public override void SetDefaults() {
  Item.width = 20; Item.height = 28; Item.maxStack = 30;
  Item.useStyle = ItemUseStyleID.DrinkLiquid; Item.useAnimation = 17; Item.useTime = 17;
  Item.UseSound = SoundID.Item3; Item.consumable = true;
  Item.healLife = 100; Item.potion = true;
  Item.rare = ItemRarityID.Green;
  Item.value = Item.sellPrice(silver: 15);
 }
 public override void AddRecipes() => CreateRecipe(2)
  .AddIngredient<VoidShard>(3).AddIngredient(ItemID.BottledWater, 2)
  .AddIngredient(ItemID.Moonglow, 1).AddTile(TileID.Bottles).Register();
}
