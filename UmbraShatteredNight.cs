using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;
using UmbraShatteredNight.Content.Systems;
using UmbraShatteredNight.Content.Items;

namespace UmbraShatteredNight;

public class UmbraShatteredNight : Mod
{
    internal enum PacketType : byte { RequestDevourerSummon = 1, RequestNightInvasion = 2 }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        PacketType kind = (PacketType)reader.ReadByte();
        if (kind != PacketType.RequestDevourerSummon && kind != PacketType.RequestNightInvasion)
            throw new IOException($"Unknown UMBRA packet: {(byte)kind}");
        // Only the server can act on summon requests. The sender is supplied by tModLoader,
        // never trusted from client-controlled packet contents.
        if (Main.netMode != NetmodeID.Server || whoAmI < 0 || whoAmI >= Main.maxPlayers)
            return;
        Player player = Main.player[whoAmI];
        if (kind == PacketType.RequestNightInvasion)
        {
            NightInvasionSystem.TryStartWithBeacon(player);
            return;
        }
        // FractureBeacon validates proximity, item ownership and payment on the server.
        FractureBeacon.TrySummonWithBeacon(player);
    }
}
