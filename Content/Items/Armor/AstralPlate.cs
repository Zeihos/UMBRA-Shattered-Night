using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Body)]
public class AstralPlate : ModItem {
 public override void SetDefaults() { Item.width=28; Item.height=26; Item.defense=17; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:3); }
 public override void UpdateEquip(Player player) { player.GetDamage(DamageClass.Generic) += 0.05f; }
 
 public override void AddRecipes() => CreateRecipe().AddIngredient<AstralBar>(20).AddTile(TileID.MythrilAnvil).Register();
}
