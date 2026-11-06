using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class ColossusGreatsword : ModItem {
 public override void SetDefaults() {
  Item.width=44; Item.height=44; Item.damage=128; Item.DamageType=DamageClass.Melee;
  Item.useTime=24; Item.useAnimation=24; Item.useStyle=ItemUseStyleID.Swing;
  Item.knockBack=7.5f; Item.autoReuse=true; Item.UseSound=SoundID.Item1;
  Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:7);
 }
 public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
  target.AddBuff(BuffID.OnFire3, 180);
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<AshenCore>(7).AddIngredient<CinderBar>(15).AddTile(TileID.MythrilAnvil).Register();
}
