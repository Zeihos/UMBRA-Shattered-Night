using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class ArchonLongbow : ModItem {
 public override void SetDefaults() {
  Item.width=44; Item.height=40; Item.damage=58; Item.DamageType=DamageClass.Ranged;
  Item.useTime=18; Item.useAnimation=18; Item.useStyle=ItemUseStyleID.Shoot;
  Item.noMelee=true; Item.knockBack=4f; Item.shoot=ProjectileID.WoodenArrowFriendly;
  Item.shootSpeed=15f; Item.useAmmo=AmmoID.Arrow; Item.UseSound=SoundID.Item5;
  Item.autoReuse=true; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:6);
 }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<ArchonHeart>(5).AddIngredient<UmbralBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
