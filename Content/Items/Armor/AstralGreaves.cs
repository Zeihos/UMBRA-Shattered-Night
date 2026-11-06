using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Legs)]
public class AstralGreaves : ModItem {
 public override void SetDefaults() { Item.width=28; Item.height=26; Item.defense=11; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:3); }
 public override void UpdateEquip(Player player) { player.moveSpeed += 0.07f; }
 
 public override void AddRecipes() => CreateRecipe().AddIngredient<AstralBar>(15).AddTile(TileID.MythrilAnvil).Register();
}
