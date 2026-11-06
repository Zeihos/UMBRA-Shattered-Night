using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class DevourerEmblem : ModItem {
 public override void SetDefaults() { Item.width=28; Item.height=28; Item.accessory=true; Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:1); }
 public override void UpdateAccessory(Player player, bool hideVisual) { player.GetDamage(DamageClass.Generic)+=.04f; player.GetCritChance(DamageClass.Generic)+=3f; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<DevourerFang>(5).AddIngredient<UmbralBar>(8).AddTile(TileID.Anvils).Register();
}
