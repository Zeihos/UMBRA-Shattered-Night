using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class AstralHeartFragment : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24; Item.height = 24; Item.maxStack = 9999;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(silver: 90);
    }
}
