using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;

public class SovereignSummonerSigil : ModItem
{
    public override void SetDefaults()
    {
        Item.width=28; Item.height=30; Item.accessory=true; Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:12);
    }
    public override void UpdateAccessory(Player player, bool hideVisual) { player.GetDamage(DamageClass.Summon) += 0.16f; player.maxMinions += 1; }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<SovereignCore>(7).AddIngredient<AstralBar>(14).AddTile(TileID.MythrilAnvil).Register();
    }
}
