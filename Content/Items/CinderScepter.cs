using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class CinderScepter : ModItem {
 public override void SetDefaults() { Item.width=40; Item.height=40; Item.damage=90; Item.useTime=22; Item.useAnimation=22; Item.autoReuse=true; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:6); Item.DamageType=DamageClass.Magic; Item.useStyle=ItemUseStyleID.Shoot; Item.noMelee=true; Item.shoot=ModContent.ProjectileType<global::UmbraShatteredNight.Content.Projectiles.CinderEmber>(); Item.shootSpeed=12f; Item.mana=13; Item.UseSound=SoundID.Item20; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<CinderBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
