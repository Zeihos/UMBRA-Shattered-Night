using Microsoft.Xna.Framework;
using Terraria.DataStructures;
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
    
    // Every third shot fires an additional astral arrow, using the selected ammunition type.
    private int arrowCounter;
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        arrowCounter = (arrowCounter + 1) % 3;
        if (arrowCounter == 0 && player.whoAmI == Main.myPlayer)
        {
            Vector2 extraVelocity = velocity.RotatedBy(MathHelper.ToRadians(7f));
            Projectile.NewProjectile(source, position, extraVelocity, type, (int)(damage * 0.65f), knockback, player.whoAmI);
        }
        return true;
    }

    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<SovereignCore>(8).AddIngredient<AstralBar>(16).AddTile(TileID.MythrilAnvil).Register();
    }
}
