using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class RiftHammer : ModItem {
 public override void SetDefaults() {
  Item.width=40; Item.height=40; Item.damage=37; Item.DamageType=DamageClass.Melee;
  Item.useStyle=ItemUseStyleID.Swing; Item.useTime=30; Item.useAnimation=30;
  Item.knockBack=7.5f; Item.UseSound=SoundID.Item1; Item.autoReuse=true;
  Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:2);
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralBar>(14).AddIngredient<DevourerFang>(4).AddTile(TileID.Anvils).Register();
}
