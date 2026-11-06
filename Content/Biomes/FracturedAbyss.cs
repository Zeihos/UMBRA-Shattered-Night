using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.Biomes;
public class FracturedAbyss : ModBiome {
 public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
 public override bool IsBiomeActive(Player player) {
  if (UmbraWorld.AbyssX <= 0 || UmbraWorld.AbyssY <= 0) return false;
  Vector2 center = new Vector2(UmbraWorld.AbyssX*16f, UmbraWorld.AbyssY*16f);
  return Vector2.DistanceSquared(player.Center, center) < 1150f*1150f;
 }
}
