using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Items;
public class AstralHeartbeat : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28; Item.height = 28; Item.accessory = true;
        Item.rare = ItemRarityID.Pink; Item.value = Item.sellPrice(gold: 3);
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Magic) += 0.08f;
        player.statManaMax2 += 40;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<AstralHeartFragment>(8)
            .AddIngredient<AstralBar>(12)
            .AddTile(TileID.MythrilAnvil).Register();
    }
}
