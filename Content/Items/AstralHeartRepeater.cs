using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Items;

// A ranged reward unlocked by defeating the Astral Heart encounter.
public class AstralHeartRepeater : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 42;
        Item.height = 20;
        Item.damage = 62;
        Item.DamageType = DamageClass.Ranged;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.knockBack = 2.5f;
        Item.value = Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Pink;
        Item.UseSound = SoundID.Item5;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.shootSpeed = 13f;
        Item.useAmmo = AmmoID.Arrow;
        Item.autoReuse = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<AstralHeartFragment>(8)
            .AddIngredient<AstralBar>(14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
