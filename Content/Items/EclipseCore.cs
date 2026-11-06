using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class EclipseCore : ModItem
{
    public override void SetDefaults() {
        Item.width = 24; Item.height = 24;
        Item.maxStack = 9999;
        Item.value = Item.sellPrice(silver: 30);
        Item.rare = ItemRarityID.Orange;
    }
}
