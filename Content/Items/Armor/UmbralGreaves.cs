using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;

namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Legs)]
public class UmbralGreaves : ModItem
{
    public override void SetDefaults() {
        Item.width = 26; Item.height = 22;
        Item.defense = 7; Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 70);
    }
    public override void UpdateEquip(Player player) { player.moveSpeed += 0.07f; }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(12)
        .AddIngredient(ItemID.Silk, 8)
        .AddTile(TileID.Anvils).Register();
}
