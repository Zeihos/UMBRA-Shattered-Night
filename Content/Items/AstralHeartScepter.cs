using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;

namespace UmbraShatteredNight.Content.Items;

// Post-Astral Heart magic reward. Uses a friendly player projectile, not a hostile NPC shot.
public class AstralHeartScepter : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 42;
        Item.damage = 68;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 14;
        Item.useTime = 23;
        Item.useAnimation = 23;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.knockBack = 3f;
        Item.shoot = ModContent.ProjectileType<ArchonArcaneShot>();
        Item.shootSpeed = 12f;
        Item.UseSound = SoundID.Item20;
        Item.autoReuse = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 6);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<AstralHeartFragment>(8)
            .AddIngredient<AstralBar>(14)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
