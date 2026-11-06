using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;

namespace UmbraShatteredNight.Content.Items;

public class AstralSurveyCharm : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 3);
        Item.rare = ItemRarityID.Pink;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (player.InModBiome<AstralRift>())
        {
            player.statDefense += 5;
            player.moveSpeed += 0.12f;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<AstralBar>(8)
            .AddIngredient<AstralGlassItem>(12)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
