using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.Items;

public class SovereignResonator : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 32;
        Item.accessory = true;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.sellPrice(gold: 12);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Generic) += 0.05f;
        player.endurance += 0.03f;
        if (UmbraWorld.DownedAstralSovereign)
            player.moveSpeed += 0.05f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SovereignCore>(6)
            .AddIngredient<AstralBar>(12)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
