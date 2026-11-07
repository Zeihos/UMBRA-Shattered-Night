using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Tiles;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>Conservative ore/biome patches: never overwrite furniture, chests,
/// temple/dungeon blocks, or tiles belonging to another UMBRA biome.</summary>
internal static class SafeVeinWorldgen
{
    public static void Patch(int centerX, int centerY, int width, int height, int targetType)
    {
        if (!WorldGen.InWorld(centerX, centerY, 100)) return;
        int radiusX = System.Math.Max(1, width / 2);
        int radiusY = System.Math.Max(1, height / 2);
        for (int dx = -radiusX; dx <= radiusX; dx++)
        for (int dy = -radiusY; dy <= radiusY; dy++)
        {
            int x = centerX + dx, y = centerY + dy;
            if (!WorldGen.InWorld(x, y, 100)) continue;
            float nx = (float)dx / radiusX, ny = (float)dy / radiusY;
            if (nx * nx + ny * ny > 1f) continue;
            Tile tile = Main.tile[x, y];
            if (!tile.HasTile || !CanReplace(tile.TileType, targetType)) continue;
            tile.TileType = (ushort)targetType;
            tile.IsHalfBlock = false;
            tile.Slope = SlopeType.Solid;
            WorldGen.SquareTileFrame(x, y, true);
        }
    }

    private static bool CanReplace(ushort existing, int target)
    {
        // Base biome tiles must be allowed to replace only vanilla stone or dirt.
        // v0.57.0 accidentally rejected ALL base biome targets here, so no biomes formed.
        if (target == ModContent.TileType<FracturedStone>() ||
            target == ModContent.TileType<AstralGlass>() ||
            target == ModContent.TileType<AshenStone>())
            return existing == TileID.Stone || existing == TileID.Dirt;

        // Ore is restricted to its own biome, not to all natural terrain.
        if (target == ModContent.TileType<UmbralOre>())
            return existing == ModContent.TileType<FracturedStone>();
        if (target == ModContent.TileType<AstralOre>())
            return existing == ModContent.TileType<AstralGlass>();
        if (target == ModContent.TileType<CinderOre>())
            return existing == ModContent.TileType<AshenStone>();
        return false;
    }
}
