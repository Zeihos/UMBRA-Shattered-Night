using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;

namespace UmbraShatteredNight;

public class UmbraShatteredNight : Mod
{
    internal enum PacketType : byte { RequestDevourerSummon = 1 }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        PacketType kind = (PacketType)reader.ReadByte();
        if (kind != PacketType.RequestDevourerSummon)
            throw new IOException($"Unknown UMBRA packet: {(byte)kind}");
        // Only the server can act on summon requests. The sender is supplied by tModLoader,
        // never trusted from client-controlled packet contents.
        if (Main.netMode != NetmodeID.Server || whoAmI < 0 || whoAmI >= Main.maxPlayers)
            return;
        Player player = Main.player[whoAmI];
        if (!player.active || player.dead || !player.ZoneRockLayerHeight ||
            NPC.AnyNPCs(ModContent.NPCType<ShardDevourer>()))
            return;
        NPC.SpawnOnPlayer(whoAmI, ModContent.NPCType<ShardDevourer>());
    }
}
