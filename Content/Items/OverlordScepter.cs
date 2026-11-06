using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;
namespace UmbraShatteredNight.Content.Items;
public class OverlordScepter : ModItem {
 public override void SetDefaults() {
  Item.width=44; Item.height=44; Item.damage=132; Item.DamageType=DamageClass.Magic;
  Item.mana=18; Item.useTime=21; Item.useAnimation=21; Item.useStyle=ItemUseStyleID.Shoot;
  Item.noMelee=true; Item.autoReuse=true; Item.knockBack=4f; Item.UseSound=SoundID.Item20;
  Item.shoot=ModContent.ProjectileType<CinderEmber>(); Item.shootSpeed=16f;
  Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:12);
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<OverlordHeart>(8).AddIngredient<CinderBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
