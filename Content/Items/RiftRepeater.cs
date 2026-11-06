using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class RiftRepeater : ModItem {
 public override void SetDefaults() {
 Item.width=40; Item.height=20; Item.damage=21; Item.DamageType=DamageClass.Ranged;
 Item.useTime=16; Item.useAnimation=16; Item.useStyle=ItemUseStyleID.Shoot;
 Item.noMelee=true; Item.knockBack=2f; Item.shoot=ProjectileID.WoodenArrowFriendly;
 Item.shootSpeed=11f; Item.useAmmo=AmmoID.Arrow; Item.UseSound=SoundID.Item5;
 Item.autoReuse=true; Item.value=Item.sellPrice(gold:1); Item.rare=ItemRarityID.Green; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralBar>(12).AddIngredient<VoidShard>(5).AddTile(TileID.Anvils).Register();
}
