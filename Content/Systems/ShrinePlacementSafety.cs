using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Tiles;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>
/// Avoid cutting generated structures (chests, altars, dungeon and temple walls,
/// furniture) when creating an UMBRA worldgen shrine. On unsuitable terrain,
/// skip the shrine rather than destroy an existing structure.
/// </summary>
internal static class ShrinePlacementSafety
{
    public static bool CanCarveNaturalArea(int centerX, int centerY, int radiusX, int radiusY, int biomeTile)
    {
        if (!WorldGen.InWorld(centerX - radiusX, centerY - radiusY, 100) ||
            !WorldGen.InWorld(centerX + radiusX, centerY + radiusY, 100))
            return false;

        for (int x = centerX - radiusX; x <= centerX + radiusX; x++)
        for (int y = centerY - radiusY; y <= centerY + radiusY; y++)
        {
            Tile tile = Main.tile[x, y];
            if (!tile.HasTile)
                continue;

            ushort type = tile.TileType;
            if (type == biomeTile || IsCommonNaturalStoneOrOre(type))
                continue;

            // This also rejects modded construction tiles. Never carve through
            // an unknown tile merely because it happens to be underground.
            return false;
        }
        return true;
    }

    private static bool IsCommonNaturalStoneOrOre(ushort type)
    {
        switch (type)
        {
            case TileID.Stone:
            case TileID.Dirt:
            case TileID.Copper:
            case TileID.Tin:
            case TileID.Iron:
            case TileID.Lead:
            case TileID.Silver:
            case TileID.Tungsten:
            case TileID.Gold:
            case TileID.Platinum:
            case TileID.Demonite:
            case TileID.Crimtane:
                return true;
        }
        // UMBRA ores can already exist if an ore-generation pass ran first.
        return type == ModContent.TileType<AstralOre>() ||
               type == ModContent.TileType<CinderOre>() ||
               type == ModContent.TileType<UmbralOre>();
    }
}
