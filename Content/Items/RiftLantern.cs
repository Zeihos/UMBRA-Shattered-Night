using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class RiftLantern : ModItem {
 public override void SetDefaults() { Item.width=24; Item.height=30; Item.accessory=true; Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:2); }
 public override void UpdateAccessory(Player player, bool hideVisual) {
  player.nightVision=true;
  player.GetDamage(DamageClass.Magic) += 0.05f;
  if (!hideVisual) Lighting.AddLight(player.Center,0.22f,0.06f,0.30f);
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralBar>(8).AddIngredient<DevourerFang>(3).AddTile(TileID.Anvils).Register();
}
