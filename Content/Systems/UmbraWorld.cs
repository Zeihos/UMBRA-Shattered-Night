using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace UmbraShatteredNight.Content.Systems;
public class UmbraWorld : ModSystem
{
    public static bool DownedSentinel;
    public static bool DownedDevourer;
    public static bool DownedNightInvasion;
    public static bool NightInvasionActive;
    public static int NightInvasionWave;
    public static int NightInvasionKills;
    public static int AbyssX, AbyssY;
    public override void OnWorldLoad() { NightInvasionSystem.ResetRuntime(); DownedSentinel = false; DownedDevourer = false; AbyssX = 0; AbyssY = 0; DownedNightInvasion = false; NightInvasionActive = false; NightInvasionWave = 0; NightInvasionKills = 0; }
    public override void OnWorldUnload() { NightInvasionSystem.ResetRuntime(); DownedSentinel = false; DownedDevourer = false; AbyssX = 0; AbyssY = 0; DownedNightInvasion = false; NightInvasionActive = false; NightInvasionWave = 0; NightInvasionKills = 0; }
    public override void SaveWorldData(TagCompound tag) {
        if (DownedSentinel) tag["downedSentinel"] = true;
        if (DownedDevourer) tag["downedDevourer"] = true;
        if (DownedNightInvasion) tag["downedNightInvasion"] = true;
        if (AbyssX > 0) { tag["abyssX"] = AbyssX; tag["abyssY"] = AbyssY; }
    }
    public override void LoadWorldData(TagCompound tag) { DownedSentinel = tag.GetBool("downedSentinel"); DownedDevourer = tag.GetBool("downedDevourer"); DownedNightInvasion = tag.GetBool("downedNightInvasion"); NightInvasionActive = false; NightInvasionWave = 0; NightInvasionKills = 0; AbyssX = tag.GetInt("abyssX"); AbyssY = tag.GetInt("abyssY"); }
    public override void NetSend(BinaryWriter writer) { writer.Write(DownedSentinel); writer.Write(DownedDevourer); writer.Write(DownedNightInvasion); writer.Write(NightInvasionActive); writer.Write(NightInvasionWave); writer.Write(NightInvasionKills); writer.Write(AbyssX); writer.Write(AbyssY); }
    public override void NetReceive(BinaryReader reader) { DownedSentinel = reader.ReadBoolean(); DownedDevourer = reader.ReadBoolean(); DownedNightInvasion = reader.ReadBoolean(); NightInvasionActive = reader.ReadBoolean(); NightInvasionWave = reader.ReadInt32(); NightInvasionKills = reader.ReadInt32(); AbyssX = reader.ReadInt32(); AbyssY = reader.ReadInt32(); }
}
