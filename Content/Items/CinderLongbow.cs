using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class CinderLongbow : ModItem {
 public override void SetDefaults() { Item.width=40; Item.height=40; Item.damage=85; Item.useTime=16; Item.useAnimation=16; Item.autoReuse=true; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:6); Item.DamageType=DamageClass.Ranged; Item.useStyle=ItemUseStyleID.Shoot; Item.noMelee=true; Item.shoot=ProjectileID.WoodenArrowFriendly; Item.useAmmo=AmmoID.Arrow; Item.shootSpeed=13f; Item.UseSound=SoundID.Item5; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<CinderBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
