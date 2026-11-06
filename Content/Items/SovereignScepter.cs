using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;
namespace UmbraShatteredNight.Content.Items;

public class SovereignScepter : ModItem
{
    public override void SetDefaults()
    {
        Item.damage=91; Item.DamageType=DamageClass.Magic; Item.width=42; Item.height=44; Item.mana=17; Item.useTime=19; Item.useAnimation=19; Item.useStyle=ItemUseStyleID.Shoot; Item.noMelee=true; Item.knockBack=3f; Item.shoot=ModContent.ProjectileType<ArchonArcaneShot>(); Item.shootSpeed=15f; Item.UseSound=SoundID.Item20; Item.autoReuse=true; Item.rare=ItemRarityID.Yellow; Item.value=Item.sellPrice(gold:12);
    }
    
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<SovereignCore>(8).AddIngredient<AstralBar>(16).AddTile(TileID.MythrilAnvil).Register();
    }
}
