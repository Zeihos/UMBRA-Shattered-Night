using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.Items;
public class SovereignBeacon : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28; Item.height = 30; Item.maxStack = 20; Item.consumable = true;
        Item.useStyle = ItemUseStyleID.HoldUp; Item.useTime = 40; Item.useAnimation = 40;
        Item.UseSound = SoundID.Roar; Item.rare = ItemRarityID.Yellow;
    }
    public override bool CanUseItem(Player player) => !Main.dayTime && UmbraWorld.DownedAstralHeart &&
        !NPC.AnyNPCs(ModContent.NPCType<AstralSovereign>());
    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            int type = ModContent.NPCType<AstralSovereign>();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            else NPC.SpawnOnPlayer(player.whoAmI, type);
        }
        return true;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<AstralHeartFragment>(10).AddIngredient<AstralBar>(15)
        .AddTile(TileID.MythrilAnvil).Register();
}
