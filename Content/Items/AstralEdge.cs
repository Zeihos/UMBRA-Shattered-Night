using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class AstralEdge : ModItem {
 public override void SetDefaults() {
  Item.damage=78; Item.DamageType=DamageClass.Melee; Item.width=44;Item.height=44;
  Item.useTime=23;Item.useAnimation=23;Item.useStyle=ItemUseStyleID.Swing;
  Item.knockBack=6f;Item.autoReuse=true;Item.UseSound=SoundID.Item1;
  Item.value=Item.sellPrice(gold:6);Item.rare=ItemRarityID.Pink;
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<AstralBar>(12).AddIngredient<ArchonHeart>(3).AddTile(TileID.MythrilAnvil).Register();
}
