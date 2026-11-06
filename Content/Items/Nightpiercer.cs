using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class Nightpiercer : ModItem
{
    public override void SetDefaults() {
        Item.damage = 18;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 38; Item.height = 18;
        Item.useTime = 20; Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.knockBack = 3f;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.shootSpeed = 10f;
        Item.useAmmo = AmmoID.Arrow;
        Item.UseSound = SoundID.Item5;
        Item.autoReuse = true;
        Item.value = Item.sellPrice(silver: 90);
        Item.rare = ItemRarityID.Blue;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(10)
        .AddIngredient(ItemID.DemoniteBar, 4)
        .AddTile(TileID.Anvils).Register();
}
