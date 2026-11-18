using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.Items;

public class SovereignBeacon : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.maxStack = 20;
        // Only the SP/server authority consumes a beacon after validation.
        Item.consumable = false;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 40;
        Item.useTime = 40;
        Item.UseSound = SoundID.Roar;
        Item.rare = ItemRarityID.Yellow;
    }

    public override bool CanUseItem(Player player) =>
        BossSummoningSystem.CanSummon(player, Type);

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI != Main.myPlayer)
            return true;

        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)global::UmbraShatteredNight.UmbraShatteredNight.PacketType.RequestSovereignSummon);
            packet.Send();
        }
        else if (Main.netMode == NetmodeID.SinglePlayer)
            BossSummoningSystem.TrySummon(player, Type);

        return true;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<AstralHeartFragment>(10)
        .AddIngredient<AstralBar>(15)
        .AddTile(TileID.MythrilAnvil).Register();
}
