using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UmbraShatteredNight.Content.Items;

// A melee reward crafted after completing the Night Invasion.
public class NightfallEdge : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 44;
        Item.height = 44;
        Item.damage = 65;
        Item.DamageType = DamageClass.Melee;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 21;
        Item.useAnimation = 21;
        Item.knockBack = 6f;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 4);
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<InvasionCrest>(1)
        .AddIngredient<VoidShard>(24)
        .AddIngredient<UmbralBar>(12)
        .AddTile(TileID.MythrilAnvil)
        .Register();
}
