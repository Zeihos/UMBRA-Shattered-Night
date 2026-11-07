using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class AstralScepter : ModItem {
 public override void SetDefaults() { Item.width=42; Item.height=42; Item.damage=51; Item.DamageType=DamageClass.Magic; Item.useTime=22; Item.useAnimation=22; Item.knockBack=3f; Item.autoReuse=true; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:5); Item.shoot=ModContent.ProjectileType<global::UmbraShatteredNight.Content.Projectiles.ArchonArcaneShot>(); Item.mana=10; Item.useStyle=ItemUseStyleID.Shoot; Item.noMelee=true; Item.shootSpeed=12f; Item.UseSound=SoundID.Item20; }
 public override void AddRecipes()=>CreateRecipe().AddIngredient<AstralBar>(16).AddIngredient<ArchonHeart>(2).AddTile(TileID.MythrilAnvil).Register();
}
