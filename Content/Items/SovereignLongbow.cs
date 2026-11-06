using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;

public class SovereignLongbow : ModItem
{
    public override void SetDefaults()
    {
        Item.damage=86; Item.DamageType=DamageClass.Ranged; Item.width=44; Item.height=22; Item.useTime=15; Item.useAnimation=15; Item.useStyle=ItemUseStyleID.Shoot; Item.noMelee=true; Item.knockBack=3.5f; Item.shoot=ProjectileID.WoodenArrowFriendly; Item.shootSpeed=16f; Item.useAmmo=AmmoID.Arrow; Item.UseSound=SoundID.Item5; Item.autoReuse=true; Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:12);
    }
    
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<SovereignCore>(8).AddIngredient<AstralBar>(16).AddTile(TileID.MythrilAnvil).Register();
    }
}
