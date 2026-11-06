using Terraria;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Items;
public class AshenStoneItem : ModItem {
 public override void SetDefaults() {
  Item.DefaultToPlaceableTile(ModContent.TileType<AshenStone>());
  Item.width = 16; Item.height = 16; Item.maxStack = 9999;
  Item.value = Item.sellPrice(copper: 75);
 }
}
