using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class SovereignCore : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24; Item.height = 26; Item.maxStack = 9999;
        Item.rare = ItemRarityID.Yellow; Item.value = Item.sellPrice(gold: 1);
    }
}
