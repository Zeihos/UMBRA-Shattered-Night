using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace UmbraShatteredNight.Content.Systems;
public class UmbraWorld : ModSystem
{
    public static bool DownedSentinel;
    public static bool DownedDevourer;
    public static bool DownedArchon;
    public static bool DownedAstralHeart;
    public static bool DownedAstralSovereign;
    public static bool DownedNightInvasion;
    public static bool NightInvasionActive;
    public static int NightInvasionWave;
    public static int NightInvasionKills;
    public static int AbyssX, AbyssY;
    public static int RiftX, RiftY;
    public override void OnWorldLoad() { NightInvasionSystem.ResetRuntime(); DownedSentinel = false; DownedDevourer = false; DownedArchon = false; DownedAstralHeart = false; DownedAstralSovereign = false; AbyssX = 0; AbyssY = 0; RiftX = 0; RiftY = 0; DownedNightInvasion = false; NightInvasionActive = false; NightInvasionWave = 0; NightInvasionKills = 0; }
    public override void OnWorldUnload() { NightInvasionSystem.ResetRuntime(); DownedSentinel = false; DownedDevourer = false; DownedArchon = false; DownedAstralHeart = false; DownedAstralSovereign = false; AbyssX = 0; AbyssY = 0; RiftX = 0; RiftY = 0; DownedNightInvasion = false; NightInvasionActive = false; NightInvasionWave = 0; NightInvasionKills = 0; }
    public override void SaveWorldData(TagCompound tag) {
        if (DownedSentinel) tag["downedSentinel"] = true;
        if (DownedDevourer) tag["downedDevourer"] = true;
        if (DownedArchon) tag["downedArchon"] = true;
        if (DownedAstralHeart) tag["downedAstralHeart"] = true;
        if (DownedAstralSovereign) tag["downedAstralSovereign"] = true;
        if (DownedNightInvasion) tag["downedNightInvasion"] = true;
        if (AbyssX > 0) { tag["abyssX"] = AbyssX; tag["abyssY"] = AbyssY; }
        if (RiftX > 0) { tag["riftX"] = RiftX; tag["riftY"] = RiftY; }
    }
    public override void LoadWorldData(TagCompound tag) { DownedSentinel = tag.GetBool("downedSentinel"); DownedDevourer = tag.GetBool("downedDevourer"); DownedArchon = tag.GetBool("downedArchon"); DownedAstralHeart = tag.GetBool("downedAstralHeart"); DownedAstralSovereign = tag.GetBool("downedAstralSovereign"); DownedNightInvasion = tag.GetBool("downedNightInvasion"); NightInvasionActive = false; NightInvasionWave = 0; NightInvasionKills = 0; AbyssX = tag.GetInt("abyssX"); AbyssY = tag.GetInt("abyssY"); RiftX = tag.GetInt("riftX"); RiftY = tag.GetInt("riftY"); }
    public override void NetSend(BinaryWriter writer) { writer.Write(DownedSentinel); writer.Write(DownedDevourer); writer.Write(DownedArchon); writer.Write(DownedAstralHeart); writer.Write(DownedAstralSovereign); writer.Write(DownedNightInvasion); writer.Write(NightInvasionActive); writer.Write(NightInvasionWave); writer.Write(NightInvasionKills); writer.Write(AbyssX); writer.Write(AbyssY); writer.Write(RiftX); writer.Write(RiftY); }
    public override void NetReceive(BinaryReader reader) { DownedSentinel = reader.ReadBoolean(); DownedDevourer = reader.ReadBoolean(); DownedArchon = reader.ReadBoolean(); DownedAstralHeart = reader.ReadBoolean(); DownedAstralSovereign = reader.ReadBoolean(); DownedNightInvasion = reader.ReadBoolean(); NightInvasionActive = reader.ReadBoolean(); NightInvasionWave = reader.ReadInt32(); NightInvasionKills = reader.ReadInt32(); AbyssX = reader.ReadInt32(); AbyssY = reader.ReadInt32(); RiftX = reader.ReadInt32(); RiftY = reader.ReadInt32(); }
}
