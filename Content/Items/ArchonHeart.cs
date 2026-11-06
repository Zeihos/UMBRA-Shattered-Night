using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class ArchonHeart : ModItem
{
    public override void SetDefaults() {
        Item.width = 26; Item.height = 30;
        Item.maxStack = 9999; Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 1);
    }
}
