using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;
namespace UmbraShatteredNight.Content.Items;
public class AstralPulseCharm : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26; Item.height = 28; Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 3);
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Generic) += 0.04f;
        if (player.InModBiome<AstralRift>()) player.statDefense += 6;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<AstralHeartFragment>(3)
        .AddIngredient<AstralBar>(12)
        .AddTile(TileID.MythrilAnvil).Register();
}
