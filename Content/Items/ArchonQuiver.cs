using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Items;

public class ArchonQuiver : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 6);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Ranged) += 0.09f; player.ammoCost80 = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<ArchonHeart>(5)
            .AddIngredient<UmbralBar>(16)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
