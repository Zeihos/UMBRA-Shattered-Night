using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class OverlordLongbow : ModItem {
 public override void SetDefaults() {
  Item.width=44; Item.height=44; Item.damage=116; Item.DamageType=DamageClass.Ranged;
  Item.useTime=15; Item.useAnimation=15; Item.useStyle=ItemUseStyleID.Shoot;
  Item.noMelee=true; Item.autoReuse=true; Item.knockBack=4f; Item.UseSound=SoundID.Item5;
  Item.useAmmo=AmmoID.Arrow; Item.shoot=ProjectileID.WoodenArrowFriendly; Item.shootSpeed=17f;
  Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:12);
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<OverlordHeart>(8).AddIngredient<CinderBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
