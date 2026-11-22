using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>
/// Keeps miniboss targets valid in multiplayer. The authority selects new
/// targets; clients wait for synced NPC.target instead of picking independently.
/// </summary>
internal static class UMBRAEnemyTargeting
{
    internal static bool TryGetTarget(NPC npc, out Player player, float maxDistance)
    {
        player = null;
        int previous = npc.target;
        if (!IsValid(npc, previous, maxDistance))
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return false;

            npc.TargetClosest(false);
            if (!IsValid(npc, npc.target, maxDistance))
                return false;

            if (npc.target != previous)
                npc.netUpdate = true;
        }

        player = Main.player[npc.target];
        return true;
    }

    private static bool IsValid(NPC npc, int index, float maxDistance)
    {
        if (index < 0 || index >= Main.maxPlayers)
            return false;

        Player candidate = Main.player[index];
        return candidate != null && candidate.active && !candidate.dead &&
            Vector2.DistanceSquared(candidate.Center, npc.Center) <= maxDistance * maxDistance;
    }
}
