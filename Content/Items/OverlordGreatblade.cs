using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class OverlordGreatblade : ModItem {
 public override void SetDefaults() {
  Item.width=46; Item.height=46; Item.damage=155; Item.DamageType=DamageClass.Melee;
  Item.useTime=21; Item.useAnimation=21; Item.useStyle=ItemUseStyleID.Swing;
  Item.knockBack=7.5f; Item.autoReuse=true; Item.UseSound=SoundID.Item1;
  Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:12);
 }
 public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) { target.AddBuff(BuffID.OnFire3,240); }
 public override void AddRecipes() => CreateRecipe().AddIngredient<OverlordHeart>(8).AddIngredient<CinderBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
