using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.Items;
public class AbyssCompass : ModItem {
 public override void SetDefaults() { Item.width=28; Item.height=28; Item.accessory=true; Item.rare=ItemRarityID.Green; Item.value=Item.sellPrice(silver:70); }
 public override void UpdateAccessory(Player player, bool hideVisual) {
  player.accWatch=3;
  if(UmbraWorld.AbyssX>0 && UmbraWorld.AbyssY>0 && !hideVisual) {
   Vector2 center=new Vector2(UmbraWorld.AbyssX*16f,UmbraWorld.AbyssY*16f);
   if(Vector2.DistanceSquared(player.Center,center)<1400f*1400f) Lighting.AddLight(player.Center,.10f,.025f,.17f);
  }
 }
 public override void AddRecipes() => CreateRecipe().AddIngredient<VoidShard>(6).AddIngredient<UmbralOreItem>(12).AddTile(TileID.Anvils).Register();
}
