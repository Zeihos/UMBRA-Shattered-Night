using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
// An upgrade requiring rewards from both early progression encounters.
public class ConvergenceBlade : ModItem
{
    public override void SetDefaults() {
        Item.width = 52; Item.height = 52;
        Item.damage = 58; Item.DamageType = DamageClass.Melee;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 24; Item.useAnimation = 24;
        Item.knockBack = 6.5f; Item.UseSound = SoundID.Item1;
        Item.autoReuse = true; Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 3);
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<ConvergenceSigil>(1)
        .AddIngredient<VoidShard>(18)
        .AddTile(TileID.Anvils).Register();
}
