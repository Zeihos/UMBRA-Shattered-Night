using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using UmbraShatteredNight.Content.Tiles;
using UmbraShatteredNight.Content.Items;
using Terraria.ID;
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
                    Main.tile[x, y].LiquidAmount = 0;
                    WorldGen.PlaceTile(x, y, glass, true, true);
                }
                else
                {
                    WorldGen.KillTile(x, y, false, false, true);
                    Main.tile[x, y].LiquidAmount = 0;
                }
            }
            // Solid floor supports a 2x2 vanilla chest at the room center.
            for (int dx = -5; dx <= 5; dx++)
            {
                int x = cx + dx, y = cy + 5;
                if (WorldGen.InWorld(x, y, 100))
                {
                    WorldGen.KillTile(x, y, false, false, true);
                    Main.tile[x, y].LiquidAmount = 0;
                    WorldGen.PlaceTile(x, y, glass, true, true);
                }
            }
            // WorldGen.PlaceChest uses the chest's bottom-left tile, not its top-left.
            // Bottom-left is (cx, cy+4), so the 2x2 chest occupies y=cy+3..cy+4.
            // Its solid supporting floor must be at cy+5.
            // Clear liquids from the actual chest footprint.
            for (int dx = 0; dx < 2; dx++)
            for (int dy = 3; dy < 5; dy++)
                Main.tile[cx + dx, cy + dy].LiquidAmount = 0;
            int chestIndex = WorldGen.PlaceChest(cx, cy + 4, (ushort)TileID.Containers, false, 0);
            if (chestIndex >= 0 && Main.chest[chestIndex] != null)
            {
                Chest chest = Main.chest[chestIndex];
                chest.item[0].SetDefaults(ModContent.ItemType<AstralReliquaryCharm>());
                chest.item[1].SetDefaults(ModContent.ItemType<AstralBar>());
                chest.item[1].stack = 7;
                chest.item[2].SetDefaults(ModContent.ItemType<AstralHeartFragment>());
                chest.item[2].stack = 2;
            }
        }));
    }
}
