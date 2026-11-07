using Terraria;
using Terraria.ID;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>
/// Server-authoritative payment for UMBRA custom-packet summoning items.
/// Both SP and the server update the actual selected stack, not a client-supplied slot.
/// </summary>
internal static class BeaconConsumption
{
    internal static bool IsHolding(Player player, int itemType)
    {
        return player != null && player.active && !player.dead &&
            player.selectedItem >= 0 && player.selectedItem < player.inventory.Length &&
            player.inventory[player.selectedItem].type == itemType &&
            player.inventory[player.selectedItem].stack > 0;
    }

    internal static bool TryConsumeHeld(Player player, int itemType)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient || !IsHolding(player, itemType))
            return false;

        Item held = player.inventory[player.selectedItem];
        if (held.stack == 1)
            held.TurnToAir();
        else
            held.stack--;

        // Synchronize the exact modified slot, including a now-empty slot.
        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.SyncEquipment, number: player.whoAmI, number2: player.selectedItem);

        return true;
    }
}
