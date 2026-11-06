using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Items;
public class FracturedStoneItem : ModItem {
 public override void SetDefaults() {
  Item.DefaultToPlaceableTile(ModContent.TileType<FracturedStone>());
  Item.width=16; Item.height=16; Item.maxStack=9999; Item.value=Item.sellPrice(copper:20);
 }
}
