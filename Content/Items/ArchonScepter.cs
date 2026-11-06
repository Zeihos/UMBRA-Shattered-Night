using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;
namespace UmbraShatteredNight.Content.Items;
public class ArchonScepter : ModItem {
 public override void SetDefaults() {
  Item.width=40; Item.height=42; Item.damage=54; Item.DamageType=DamageClass.Magic;
  Item.mana=12; Item.useTime=24; Item.useAnimation=24; Item.useStyle=ItemUseStyleID.Shoot;
  Item.noMelee=true; Item.knockBack=3f; Item.shoot=ModContent.ProjectileType<UmbralBolt>();
  Item.shootSpeed=11f; Item.UseSound=SoundID.Item20; Item.autoReuse=true;
  Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:6);
 }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<ArchonHeart>(5).AddIngredient<UmbralBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
