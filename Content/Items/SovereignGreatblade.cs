using Terraria;
using UmbraShatteredNight.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;

public class SovereignGreatblade : ModItem
{
    public override void SetDefaults()
    {
        Item.shoot=ModContent.ProjectileType<SovereignBladeWave>(); Item.shootSpeed=13f; Item.damage=108; Item.DamageType=DamageClass.Melee; Item.width=48; Item.height=48; Item.useTime=19; Item.useAnimation=19; Item.useStyle=ItemUseStyleID.Swing; Item.knockBack=7f; Item.autoReuse=true; Item.UseSound=SoundID.Item1; Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:12);
    }
    
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<SovereignCore>(8).AddIngredient<AstralBar>(16).AddTile(TileID.MythrilAnvil).Register();
    }
}
