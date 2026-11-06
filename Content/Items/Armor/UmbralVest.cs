using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;

namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Body)]
public class UmbralVest : ModItem
{
    public override void SetDefaults() {
        Item.width = 26; Item.height = 22;
        Item.defense = 8; Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 70);
    }
    public override void UpdateEquip(Player player) { player.statManaMax2 += 20; }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(18)
        .AddIngredient(ItemID.Silk, 12)
        .AddTile(TileID.Anvils).Register();
}
