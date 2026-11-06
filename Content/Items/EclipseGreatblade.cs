using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class EclipseGreatblade : ModItem
{
    public override void SetDefaults() {
        Item.width = 54; Item.height = 54;
        Item.damage = 42; Item.DamageType = DamageClass.Melee;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 25; Item.useAnimation = 25;
        Item.knockBack = 6f; Item.UseSound = SoundID.Item1;
        Item.autoReuse = true; Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 2);
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<EclipseCore>(5)
        .AddIngredient<VoidShard>(15)
        .AddTile(TileID.Anvils).Register();
}
