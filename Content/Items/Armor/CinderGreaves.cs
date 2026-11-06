using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Legs)]
public class CinderGreaves : ModItem {
 public override void SetDefaults() { Item.width=28; Item.height=28; Item.defense=14; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:5); }
 public override void UpdateEquip(Player player) { player.moveSpeed += 0.09f; }
 
 public override void AddRecipes() => CreateRecipe().AddIngredient<CinderBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
