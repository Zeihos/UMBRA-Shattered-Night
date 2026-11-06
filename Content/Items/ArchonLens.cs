using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Items;

public class ArchonLens : ModItem
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
        player.GetDamage(DamageClass.Magic) += 0.08f; player.GetCritChance(DamageClass.Magic) += 6f;
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
