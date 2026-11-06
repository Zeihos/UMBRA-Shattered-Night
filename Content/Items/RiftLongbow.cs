using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class RiftLongbow : ModItem {
 public override void SetDefaults() {
  Item.width=44; Item.height=24; Item.damage=29; Item.DamageType=DamageClass.Ranged;
  Item.useTime=25; Item.useAnimation=25; Item.useStyle=ItemUseStyleID.Shoot;
  Item.noMelee=true; Item.knockBack=3f; Item.shoot=ProjectileID.WoodenArrowFriendly;
  Item.shootSpeed=13f; Item.useAmmo=AmmoID.Arrow; Item.UseSound=SoundID.Item5;
  Item.autoReuse=true; Item.value=Item.sellPrice(gold:1); Item.rare=ItemRarityID.Orange;
 }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<UmbralBar>(14).AddIngredient<DevourerFang>(5).AddTile(TileID.Anvils).Register();
}
