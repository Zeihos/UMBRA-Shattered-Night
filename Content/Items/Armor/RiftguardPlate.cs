using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Body)]
public class RiftguardPlate : ModItem {
 public override void SetDefaults() { Item.width=26; Item.height=24; Item.defense=12; Item.rare=ItemRarityID.Orange; Item.value=Item.sellPrice(gold:1); }
 public override void UpdateEquip(Player player) { player.endurance += 0.03f; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<UmbralBar>(16).AddIngredient<DevourerFang>(7).AddTile(TileID.Anvils).Register();
}
