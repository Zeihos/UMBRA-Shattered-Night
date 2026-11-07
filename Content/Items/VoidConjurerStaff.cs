using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;
namespace UmbraShatteredNight.Content.Items;
public class VoidConjurerStaff : ModItem {
 public override void SetDefaults() {
  Item.width=40; Item.height=40; Item.damage=18; Item.DamageType=DamageClass.Summon;
  Item.mana=10; Item.useTime=30; Item.useAnimation=30; Item.useStyle=ItemUseStyleID.Swing;
  Item.noMelee=true; Item.knockBack=2f; Item.shoot=ModContent.ProjectileType<VoidMinion>();
  Item.shootSpeed=0f; Item.UseSound=SoundID.Item44; Item.buffType=ModContent.BuffType<global::UmbraShatteredNight.Content.Buffs.VoidMinionBuff>();
  Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:1);
 }
 public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback) { player.AddBuff(Item.buffType,3600); Projectile.NewProjectile(source, player.Center, Microsoft.Xna.Framework.Vector2.Zero, type, damage, knockback, player.whoAmI); return false; }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<UmbralBar>(12).AddIngredient<VoidShard>(14).AddTile(TileID.Anvils).Register();
}
