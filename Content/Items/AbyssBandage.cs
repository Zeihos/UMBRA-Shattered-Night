using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Items;

public class AbyssBandage : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 22;
        Item.maxStack = Item.CommonMaxStack;
        Item.consumable = true;
        Item.useStyle = ItemUseStyleID.EatFood;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.UseSound = SoundID.Item3;
        Item.healLife = 45;
        Item.potion = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.buyPrice(silver: 2);
    }

    public override void AddRecipes()
    {
        CreateRecipe(2)
            .AddIngredient<VoidShard>(2)
            .AddIngredient(ItemID.Silk, 1)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
