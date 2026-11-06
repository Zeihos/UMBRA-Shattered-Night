using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Body)]
public class CinderPlate : ModItem {
 public override void SetDefaults() { Item.width=28; Item.height=28; Item.defense=22; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:5); }
 public override void UpdateEquip(Player player) { player.GetDamage(DamageClass.Generic) += 0.06f; }
 
 public override void AddRecipes() => CreateRecipe().AddIngredient<CinderBar>(24).AddTile(TileID.MythrilAnvil).Register();
}
