using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Buffs;
using UmbraShatteredNight.Content.Projectiles;
namespace UmbraShatteredNight.Content.Items;
public class OverlordSummonerStaff : ModItem {
 public override void SetDefaults() {
  Item.width=44; Item.height=44; Item.damage=88; Item.DamageType=DamageClass.Summon;
  Item.mana=14; Item.useTime=25; Item.useAnimation=25; Item.useStyle=ItemUseStyleID.Swing;
  Item.noMelee=true; Item.knockBack=3f; Item.UseSound=SoundID.Item44;
  Item.shoot=ModContent.ProjectileType<OverlordInfernoMinion>(); Item.shootSpeed=0f;
  Item.buffType=ModContent.BuffType<OverlordInfernoBuff>();
  Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:12);
 }
 public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
  player.AddBuff(Item.buffType,3600);
  if(player.whoAmI==Main.myPlayer) Projectile.NewProjectile(source,player.Center,Vector2.Zero,type,damage,knockback,player.whoAmI);
  return false;
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<OverlordHeart>(8).AddIngredient<CinderBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
