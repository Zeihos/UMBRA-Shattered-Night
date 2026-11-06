using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class AbyssSabre : ModItem {
 public override void SetDefaults() {
 Item.width=42; Item.height=42; Item.damage=26; Item.DamageType=DamageClass.Melee;
 Item.useTime=19; Item.useAnimation=19; Item.useStyle=ItemUseStyleID.Swing;
 Item.knockBack=4f; Item.UseSound=SoundID.Item1; Item.autoReuse=true;
 Item.value=Item.sellPrice(gold:1); Item.rare=ItemRarityID.Green; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralBar>(10).AddIngredient<VoidShard>(4).AddTile(TileID.Anvils).Register();
}
