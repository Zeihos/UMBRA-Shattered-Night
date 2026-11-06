using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class UmbralCharm : ModItem
{
    public override void SetDefaults() {
        Item.width = 26; Item.height = 28;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Green;
    }
    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.GetDamage(DamageClass.Generic) += 0.04f;
        if (!Main.dayTime) player.moveSpeed += 0.08f;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(15)
        .AddIngredient(ItemID.FallenStar, 3)
        .AddTile(TileID.Anvils).Register();
}
