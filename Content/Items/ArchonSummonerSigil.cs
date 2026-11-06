using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class ArchonSummonerSigil : ModItem {
 public override void SetDefaults() {
  Item.width=28; Item.height=30; Item.accessory=true; Item.rare=ItemRarityID.Pink;
  Item.value=Item.sellPrice(gold:6);
 }
 public override void UpdateAccessory(Player player, bool hideVisual) {
  player.GetDamage(DamageClass.Summon) += 0.12f;
  player.maxMinions += 1;
 }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<ArchonHeart>(4).AddIngredient<UmbralBar>(14).AddTile(TileID.MythrilAnvil).Register();
}
