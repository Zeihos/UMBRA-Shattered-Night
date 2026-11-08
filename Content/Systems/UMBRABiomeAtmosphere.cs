using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;

namespace UmbraShatteredNight.Content.Systems;

/// <summary>
/// Restrained, biome-specific atmosphere for the three existing UMBRA biomes.
/// Dust and light are local cosmetic effects: no packets, damage or world edits.
/// </summary>
public sealed class UMBRABiomeAtmosphere : ModPlayer
{
    public override void PostUpdate()
    {
        // ModPlayer.PostUpdate executes for remote players and on servers too.
        // Never create client particles or lighting on a dedicated server.
        if (Main.dedServ || Main.gameMenu || Player.whoAmI != Main.myPlayer ||
            !Player.active || Player.dead)
            return;

        // One chance per 18 ticks: at most one ambient particle per update.
        if (!Main.rand.NextBool(18))
            return;

        int biome = Player.InModBiome<FracturedAbyss>() ? 1 :
            Player.InModBiome<AstralRift>() ? 2 :
            Player.InModBiome<AshenWastes>() ? 3 : 0;
        if (biome == 0 || Main.screenWidth < 144 || Main.screenHeight < 144)
            return;

        // Spawn inside the visible viewport rather than behind the player.
        Vector2 position = Main.screenPosition + new Vector2(
            Main.rand.Next(72, Main.screenWidth - 72),
            Main.rand.Next(72, Main.screenHeight - 72));
        if (Vector2.DistanceSquared(position, Player.Center) > 880f * 880f)
            return;

        int tileX = (int)(position.X / 16f);
        int tileY = (int)(position.Y / 16f);
        if (!WorldGen.InWorld(tileX, tileY, 10))
            return;

        Tile tile = Framing.GetTileSafely(tileX, tileY);
        // Dust must drift through open air, not solid blocks or liquid.
        if ((tile.HasTile && Main.tileSolid[tile.TileType]) || tile.LiquidAmount > 0)
            return;

        int dustType;
        Vector2 velocity;
        float glowR;
        float glowG;
        float glowB;
        if (biome == 1) // Fractured Abyss: slow violet motes.
        {
            dustType = DustID.PurpleTorch;
            velocity = new Vector2(Main.rand.NextFloat(-0.22f, 0.22f),
                                   Main.rand.NextFloat(-0.38f, -0.10f));
            glowR = 0.055f; glowG = 0.012f; glowB = 0.095f;
        }
        else if (biome == 2) // Astral Rift: gently drifting blue stars.
        {
            dustType = DustID.BlueTorch;
            velocity = new Vector2(Main.rand.NextFloat(-0.32f, 0.32f),
                                   Main.rand.NextFloat(-0.50f, -0.20f));
            glowR = 0.025f; glowG = 0.070f; glowB = 0.13f;
        }
        else // Ashen Wastes: mixed drifting ash and occasional embers.
        {
            bool ember = Main.rand.NextBool(4);
            dustType = ember ? DustID.Torch : DustID.Smoke;
            velocity = new Vector2(Main.rand.NextFloat(-0.45f, 0.45f),
                                   Main.rand.NextFloat(-0.6f, -0.12f));
            glowR = ember ? 0.14f : 0.022f;
            glowG = ember ? 0.034f : 0.015f;
            glowB = ember ? 0.006f : 0.012f;
        }

        int index = Dust.NewDust(position - new Vector2(2f, 2f), 4, 4, dustType);
        Dust dust = Main.dust[index];
        dust.position = position;
        dust.velocity = velocity;
        dust.noGravity = true;
        dust.scale = Main.rand.NextFloat(0.55f, 0.90f);
        dust.alpha = Main.rand.Next(80, 145);
        // Dim local accent; this does not illuminate the whole biome or any tiles.
        Lighting.AddLight(position, glowR, glowG, glowB);
    }
}
