using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;

namespace UmbraShatteredNight.Content.Items.Armor;
[AutoloadEquip(EquipType.Head)]
public class UmbralHood : ModItem
{
    public override void SetDefaults() {
        Item.width = 26; Item.height = 22;
        Item.defense = 7; Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 70);
    }
    public override void UpdateEquip(Player player) { player.GetDamage(DamageClass.Magic) += 0.04f; }
    public override bool IsArmorSet(Item head, Item body, Item legs) =>
        head.type == ModContent.ItemType<UmbralHood>() &&
        body.type == ModContent.ItemType<UmbralVest>() &&
        legs.type == ModContent.ItemType<UmbralGreaves>();
    public override void UpdateArmorSet(Player player) {
        player.setBonus = "Umbral focus: +6% magic damage and +20 max mana";
        player.GetDamage(DamageClass.Magic) += 0.06f;
        player.statManaMax2 += 20;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(12)
        .AddIngredient(ItemID.Silk, 8)
        .AddTile(TileID.Anvils).Register();
}
