using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Systems;
public class AstralShrineWorldgen : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int index = tasks.FindIndex(pass => pass.Name == "UMBRA: Astral Rift");
        if (index < 0) return;
        tasks.Insert(index + 1, new PassLegacy("UMBRA: Astral Shrine", (progress, config) =>
        {
            progress.Message = "Forming the Astral Shrine";
            int cx = UmbraWorld.RiftX, cy = UmbraWorld.RiftY;
            if (!WorldGen.InWorld(cx, cy, 100)) return;
            int glass = ModContent.TileType<AstralGlass>();
            // A small open chamber of astral glass; generated only in fresh worlds.
            for (int dx = -12; dx <= 12; dx++)
            for (int dy = -8; dy <= 8; dy++)
            {
                int x = cx + dx, y = cy + dy;
                if (!WorldGen.InWorld(x, y, 100)) continue;
                bool border = Math.Abs(dx) == 12 || Math.Abs(dy) == 8;
                if (border)
                {
                    WorldGen.KillTile(x, y, false, false, true);
                    WorldGen.PlaceTile(x, y, glass, true, true);
                }
                else WorldGen.KillTile(x, y, false, false, true);
            }
            for (int dx = -3; dx <= 3; dx++)
            {
                int x = cx + dx, y = cy + 5;
                WorldGen.PlaceTile(x, y, glass, true, true);
            }
        }));
    }
}
