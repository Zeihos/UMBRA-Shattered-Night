using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.Biomes;
public class AshenWastes : ModBiome {
 public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
 public override bool IsBiomeActive(Player player) {
  if (UmbraWorld.AshenX <= 0 || UmbraWorld.AshenY <= 0) return false;
  Vector2 center = new Vector2(UmbraWorld.AshenX * 16f, UmbraWorld.AshenY * 16f);
  return Vector2.DistanceSquared(player.Center, center) < 900f * 900f;
 }
}
