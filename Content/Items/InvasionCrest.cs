using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Items;

// First-clear invasion trophy and crafting component.
public class InvasionCrest : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.maxStack = 9999;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(silver: 75);
    }
}
