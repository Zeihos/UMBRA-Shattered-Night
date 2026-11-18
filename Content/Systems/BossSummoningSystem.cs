using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.NPCs;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>
/// Authoritative boss summoning for all four progression beacons.
/// Clients send only a fixed request type: the server validates its own player
/// inventory, world progression and active bosses before consuming the beacon.
/// </summary>
internal static class BossSummoningSystem
{
    internal static bool CanSummon(Player player, int itemType)
    {
        if (!BeaconConsumption.IsHolding(player, itemType) ||
            Main.dayTime || UmbraWorld.NightInvasionActive)
            return false;

        int bossType = BossTypeForItem(itemType);
        if (bossType < 0 || NPC.AnyNPCs(bossType))
            return false;

        if (itemType == ModContent.ItemType<ArchonBeacon>())
            return UmbraWorld.DownedNightInvasion;
        if (itemType == ModContent.ItemType<SovereignBeacon>())
            return UmbraWorld.DownedAstralHeart;
        if (itemType == ModContent.ItemType<OverlordEmberSeal>())
            return UmbraWorld.DownedAshenColossus;

        // NightSigil is the first boss summon and has no earlier boss gate.
        return itemType == ModContent.ItemType<NightSigil>();
    }

    internal static bool TrySummon(Player player, int itemType)
    {
        // Never use the sender's claimed slot, boss ID, or client-side CanUseItem
        // as authority. The server reads the actual selected stack and world.
        if (Main.netMode == NetmodeID.MultiplayerClient || !CanSummon(player, itemType))
            return false;

        int bossType = BossTypeForItem(itemType);
        if (bossType < 0 || !BeaconConsumption.TryConsumeHeld(player, itemType))
            return false;

        NPC.SpawnOnPlayer(player.whoAmI, bossType);
        return true;
    }

    private static int BossTypeForItem(int itemType)
    {
        if (itemType == ModContent.ItemType<NightSigil>())
            return ModContent.NPCType<EclipseSentinel>();
        if (itemType == ModContent.ItemType<ArchonBeacon>())
            return ModContent.NPCType<RiftArchon>();
        if (itemType == ModContent.ItemType<SovereignBeacon>())
            return ModContent.NPCType<AstralSovereign>();
        if (itemType == ModContent.ItemType<OverlordEmberSeal>())
            return ModContent.NPCType<AshenOverlord>();
        return -1;
    }
}
