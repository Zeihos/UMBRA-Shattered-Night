using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class FangLance : ModItem {
 public override void SetDefaults() {
  Item.width=44; Item.height=44; Item.damage=41; Item.DamageType=DamageClass.Melee;
  Item.useStyle=ItemUseStyleID.Swing; Item.useAnimation=24; Item.useTime=24;
  Item.knockBack=5f; Item.UseSound=SoundID.Item1; Item.autoReuse=true;
  Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:1);
 }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<DevourerFang>(4).AddIngredient<UmbralBar>(8).AddTile(TileID.Anvils).Register();
}
