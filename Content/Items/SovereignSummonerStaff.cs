using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;

namespace UmbraShatteredNight.Content.Items;

public class SovereignSummonerStaff : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.damage = 57;
        Item.DamageType = DamageClass.Summon;
        Item.mana = 12;
        Item.useTime = 28;
        Item.useAnimation = 28;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.knockBack = 3f;
        Item.shoot = ModContent.ProjectileType<SovereignMinion>();
        Item.shootSpeed = 0f;
        Item.UseSound = SoundID.Item44;
        Item.buffType = ModContent.BuffType<UmbraShatteredNight.Content.Buffs.SovereignMinionBuff>();
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.sellPrice(gold: 12);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        player.AddBuff(Item.buffType, 3600);
        if (player.whoAmI == Main.myPlayer)
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
        return false;
    }

    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient<SovereignCore>(8).AddIngredient<AstralBar>(16)
            .AddTile(TileID.MythrilAnvil).Register();
    }
}
