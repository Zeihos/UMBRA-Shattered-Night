using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class ColossusLongbow : ModItem {
 public override void SetDefaults() {
  Item.width=40; Item.height=40; Item.damage=99; Item.DamageType=DamageClass.Ranged;
  Item.useTime=17; Item.useAnimation=17; Item.useStyle=ItemUseStyleID.Shoot;
  Item.noMelee=true; Item.knockBack=3f; Item.UseSound=SoundID.Item5;
  Item.shoot=ProjectileID.WoodenArrowFriendly; Item.shootSpeed=14f;
  Item.useAmmo=AmmoID.Arrow; Item.autoReuse=true; Item.rare=ItemRarityID.Yellow;
  Item.value=Item.sellPrice(gold:7);
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<AshenCore>(7).AddIngredient<CinderBar>(15).AddTile(TileID.MythrilAnvil).Register();
}
