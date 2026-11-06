using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class AstralLongbow : ModItem {
 public override void SetDefaults() { Item.width=42; Item.height=42; Item.damage=49; Item.DamageType=DamageClass.Ranged; Item.useTime=18; Item.useAnimation=18; Item.knockBack=3f; Item.autoReuse=true; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:5); Item.shoot=ProjectileID.WoodenArrowFriendly; Item.useAmmo=AmmoID.Arrow; Item.useStyle=ItemUseStyleID.Shoot; Item.noMelee=true; Item.shootSpeed=15f; Item.UseSound=SoundID.Item5; }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<AstralBar>(16).AddIngredient<ArchonHeart>(2).AddTile(TileID.MythrilAnvil).Register();
}
