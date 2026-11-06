using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class VoidShard : ModItem
{
    public override void SetDefaults() {
        Item.width = 20; Item.height = 22;
        Item.maxStack = 9999;
        Item.value = Item.sellPrice(silver: 3);
        Item.rare = ItemRarityID.Blue;
    }
}
