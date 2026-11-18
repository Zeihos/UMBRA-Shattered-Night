using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.Items;

public class NightSigil : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 32;
        Item.maxStack = 20;
        // Only the SP/server authority consumes a beacon after validation.
        Item.consumable = false;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 45;
        Item.useTime = 45;
        Item.UseSound = SoundID.Roar;
        Item.rare = ItemRarityID.Green;
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
            packet.Write((byte)global::UmbraShatteredNight.UmbraShatteredNight.PacketType.RequestSentinelSummon);
            packet.Send();
        }
        else if (Main.netMode == NetmodeID.SinglePlayer)
            BossSummoningSystem.TrySummon(player, Type);

        return true;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(12)
        .AddIngredient(ItemID.FallenStar, 2)
        .AddTile(TileID.DemonAltar).Register();
}
