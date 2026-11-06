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
            tile.Slope = Terraria.Enums.SlopeType.Solid;
            WorldGen.SquareTileFrame(x, y, true);
        }
    }

    private static bool CanReplace(ushort existing, int target)
    {
        if (existing == TileID.Stone || existing == TileID.Dirt) return true;
        if (target == ModContent.TileType<UmbralOre>())
            return existing == ModContent.TileType<FracturedStone>();
        if (target == ModContent.TileType<AstralOre>())
            return existing == ModContent.TileType<AstralGlass>();
        if (target == ModContent.TileType<CinderOre>())
            return existing == ModContent.TileType<AshenStone>();
        return false;
    }
}
