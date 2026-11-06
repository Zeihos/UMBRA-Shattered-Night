using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.Items;
public class OverlordEmberSeal : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28; Item.height = 30; Item.maxStack = 20; Item.consumable = true;
        Item.useStyle = ItemUseStyleID.HoldUp; Item.useTime = 40; Item.useAnimation = 40;
        Item.UseSound = SoundID.Roar; Item.rare = ItemRarityID.Yellow;
    }
    public override bool CanUseItem(Player player) => !Main.dayTime && UmbraWorld.DownedAshenColossus &&
        !NPC.AnyNPCs(ModContent.NPCType<AshenOverlord>());
    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            int type = ModContent.NPCType<AshenOverlord>();
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            else NPC.SpawnOnPlayer(player.whoAmI, type);
        }
        return true;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<AshenCore>(8).AddIngredient<CinderBar>(18)
        .AddTile(TileID.MythrilAnvil).Register();
}
