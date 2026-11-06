using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class ArchonBlade : ModItem
{
    public override void SetDefaults() {
        Item.width = 44; Item.height = 48; Item.damage = 82;
        Item.DamageType = DamageClass.Melee; Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 19; Item.useTime = 19; Item.knockBack = 7f;
        Item.UseSound = SoundID.Item1; Item.autoReuse = true;
        Item.rare = ItemRarityID.Pink; Item.value = Item.sellPrice(gold: 6);
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<ArchonHeart>(5).AddIngredient<UmbralBar>(18)
        .AddTile(TileID.MythrilAnvil).Register();
}
