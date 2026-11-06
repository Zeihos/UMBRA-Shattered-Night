using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Items;
public class CinderGreatsword : ModItem {
 public override void SetDefaults() { Item.width=40; Item.height=40; Item.damage=110; Item.useTime=20; Item.useAnimation=20; Item.autoReuse=true; Item.rare=ItemRarityID.Pink; Item.value=Item.sellPrice(gold:6); Item.DamageType=DamageClass.Melee; Item.useStyle=ItemUseStyleID.Swing; Item.UseSound=SoundID.Item1; Item.knockBack=7f; }
 public override void AddRecipes() => CreateRecipe().AddIngredient<CinderBar>(18).AddTile(TileID.MythrilAnvil).Register();
}
