using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
using UmbraShatteredNight.Content.NPCs;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>
/// Server-authoritative summoning for UMBRA bosses. Two players submitting
/// requests in one update tick cannot consume two summoning items.
/// </summary>
internal static class BossSummoningSystem
{
    private static ulong lastSuccessfulSummonTick = ulong.MaxValue;
    internal static void ResetRuntime() => lastSuccessfulSummonTick = ulong.MaxValue;

    // Naturally-spawning Astral Heart and Ashen Colossus are excluded so
    // unrelated distant biome encounters do not block the whole world.
    internal static bool HasActiveUmbraBoss() =>
        NPC.AnyNPCs(ModContent.NPCType<EclipseSentinel>()) ||
        NPC.AnyNPCs(ModContent.NPCType<ShardDevourer>()) ||
        NPC.AnyNPCs(ModContent.NPCType<RiftArchon>()) ||
        NPC.AnyNPCs(ModContent.NPCType<AstralSovereign>()) ||
        NPC.AnyNPCs(ModContent.NPCType<AshenOverlord>());

    private static bool HasFreeNpcSlot()
    {
        for (int i = 0; i < Main.maxNPCs; i++)
            if (!Main.npc[i].active)
                return true;
        return false;
    }

    private static bool CanProcessSummon() =>
        lastSuccessfulSummonTick != (ulong)Main.GameUpdateCount && HasFreeNpcSlot();

    internal static bool CanSummon(Player player, int itemType)
    {
        if (!BeaconConsumption.IsHolding(player, itemType) ||
            Main.dayTime || UmbraWorld.NightInvasionActive || HasActiveUmbraBoss())
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
        return itemType == ModContent.ItemType<NightSigil>();
    }

    internal static bool TrySummon(Player player, int itemType)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient || !CanSummon(player, itemType))
            return false;
        if (!CanProcessSummon())
            return false;

        int bossType = BossTypeForItem(itemType);
        if (bossType < 0 || !BeaconConsumption.TryConsumeHeld(player, itemType))
            return false;

        // Reserve the current tick before creating the NPC. A second request
        // cannot spend another item even before the spawn is replicated.
        lastSuccessfulSummonTick = (ulong)Main.GameUpdateCount;
        NPC.SpawnOnPlayer(player.whoAmI, bossType);
        return true;
    }

    internal static bool CanSummonDevourer(Player player) =>
        BeaconConsumption.IsHolding(player, ModContent.ItemType<FractureBeacon>()) &&
        player.ZoneRockLayerHeight && !UmbraWorld.NightInvasionActive &&
        !HasActiveUmbraBoss();

    internal static bool TrySummonDevourer(Player player)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient || !CanSummonDevourer(player))
            return false;
        if (!CanProcessSummon() ||
            !BeaconConsumption.TryConsumeHeld(player, ModContent.ItemType<FractureBeacon>()))
            return false;

        lastSuccessfulSummonTick = (ulong)Main.GameUpdateCount;
        NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ShardDevourer>());
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
