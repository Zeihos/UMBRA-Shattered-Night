using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.Items;
public class NightInvasionBeacon : ModItem
{
    // Uses its own Content/Items/NightInvasionBeacon.png sprite.
    public override void SetDefaults()
    {
        Item.width = 24; Item.height = 24;
        Item.maxStack = 99; Item.consumable = false; // Consumed by authoritative event-start logic.
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 30; Item.useAnimation = 30;
        Item.rare = ItemRarityID.Orange;
        Item.UseSound = SoundID.Item4;
    }
    public override bool CanUseItem(Player player) =>
        BeaconConsumption.IsHolding(player, Type) && player.ZoneOverworldHeight &&
        UmbraWorld.DownedSentinel && !Main.dayTime && !UmbraWorld.NightInvasionActive;
    public override bool? UseItem(Player player)
    {
        if (Main.netMode == NetmodeID.SinglePlayer) NightInvasionSystem.TryStartWithBeacon(player);
        else if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)UmbraShatteredNight.PacketType.RequestNightInvasion);
            packet.Send();
        }
        return true;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(20)
        .AddIngredient<EclipseCore>(1)
        .AddTile(TileID.DemonAltar).Register();
}
