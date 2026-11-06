using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;
namespace UmbraShatteredNight.Content.Items;
public class ColossusScepter : ModItem {
 public override void SetDefaults() {
  Item.width=40; Item.height=40; Item.damage=112;
  Item.DamageType=DamageClass.Magic; Item.mana=16;
  Item.useTime=23; Item.useAnimation=23; Item.useStyle=ItemUseStyleID.Shoot;
  Item.noMelee=true; Item.autoReuse=true; Item.knockBack=4f;
  Item.shoot=ModContent.ProjectileType<CinderEmber>(); Item.shootSpeed=14f;
  Item.UseSound=SoundID.Item20; Item.rare=ItemRarityID.Yellow;
  Item.value=Item.sellPrice(gold:9);
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<AshenCore>(7).AddIngredient<CinderBar>(15).AddTile(TileID.MythrilAnvil).Register();
}
