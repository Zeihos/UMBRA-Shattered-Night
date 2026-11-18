using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;
using UmbraShatteredNight.Content.Items;

namespace UmbraShatteredNight;

public class UmbraShatteredNight : Mod
{
    internal enum PacketType : byte
    {
        RequestDevourerSummon = 1,
        RequestNightInvasion = 2,
        RequestSentinelSummon = 3,
        RequestArchonSummon = 4,
        RequestSovereignSummon = 5,
        RequestOverlordSummon = 6
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        PacketType kind = (PacketType)reader.ReadByte();
        if (kind != PacketType.RequestDevourerSummon &&
            kind != PacketType.RequestNightInvasion &&
            kind != PacketType.RequestSentinelSummon &&
            kind != PacketType.RequestArchonSummon &&
            kind != PacketType.RequestSovereignSummon &&
            kind != PacketType.RequestOverlordSummon)
            throw new IOException($"Unknown UMBRA packet: {(byte)kind}");

        // The sender index comes from tModLoader, never from packet contents.
        // All item and world-state checks are performed on the authority.
        if (Main.netMode != NetmodeID.Server || whoAmI < 0 || whoAmI >= Main.maxPlayers)
            return;

        Player player = Main.player[whoAmI];
        switch (kind)
        {
            case PacketType.RequestDevourerSummon:
                FractureBeacon.TrySummonWithBeacon(player);
                break;
            case PacketType.RequestNightInvasion:
                NightInvasionSystem.TryStartWithBeacon(player);
                break;
            case PacketType.RequestSentinelSummon:
                BossSummoningSystem.TrySummon(player, ModContent.ItemType<NightSigil>());
                break;
            case PacketType.RequestArchonSummon:
                BossSummoningSystem.TrySummon(player, ModContent.ItemType<ArchonBeacon>());
                break;
            case PacketType.RequestSovereignSummon:
                BossSummoningSystem.TrySummon(player, ModContent.ItemType<SovereignBeacon>());
                break;
            case PacketType.RequestOverlordSummon:
                BossSummoningSystem.TrySummon(player, ModContent.ItemType<OverlordEmberSeal>());
                break;
        }
    }
}
