using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;

namespace UmbraShatteredNight.Content.Items;

public class AstralHeartAegis : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 4);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.endurance += 0.04f;
        if (player.InModBiome<AstralRift>()) player.statDefense += 4;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<AstralHeartFragment>(6)
            .AddIngredient<AstralBar>(10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
