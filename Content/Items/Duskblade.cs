using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class Duskblade : ModItem
{
    public override void SetDefaults() {
        Item.damage = 24;
        Item.DamageType = DamageClass.Melee;
        Item.width = 40; Item.height = 40;
        Item.useTime = 22; Item.useAnimation = 22;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 5f;
        Item.value = Item.sellPrice(silver: 90);
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(12)
        .AddIngredient(ItemID.DemoniteBar, 5)
        .AddTile(TileID.Anvils).Register();
}
