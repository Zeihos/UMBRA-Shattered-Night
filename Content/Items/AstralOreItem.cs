using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Items;
public class AstralOreItem : ModItem {
 public override void SetDefaults() {
  Item.DefaultToPlaceableTile(ModContent.TileType<AstralOre>());
  Item.width=16;Item.height=16;Item.maxStack=9999;
  Item.value=Item.sellPrice(silver:14); Item.rare=ItemRarityID.LightRed;
 }
}
