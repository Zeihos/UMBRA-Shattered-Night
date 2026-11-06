using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Legs)]
public class RiftguardBoots : ModItem {
 public override void SetDefaults() { Item.width=26; Item.height=24; Item.defense=8; Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:1); }
 public override void UpdateEquip(Player player) { player.moveSpeed += 0.06f; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralBar>(12).AddIngredient<DevourerFang>(4).AddTile(TileID.Anvils).Register();
}
