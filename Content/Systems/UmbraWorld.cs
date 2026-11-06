using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace UmbraShatteredNight.Content.Systems;
public class UmbraWorld : ModSystem
{
    public static bool DownedSentinel;
    public static bool DownedDevourer;
    public static int AbyssX, AbyssY;
    public override void OnWorldLoad() { DownedSentinel = false; DownedDevourer = false; AbyssX = 0; AbyssY = 0; }
    public override void OnWorldUnload() { DownedSentinel = false; DownedDevourer = false; AbyssX = 0; AbyssY = 0; }
    public override void SaveWorldData(TagCompound tag) {
        if (DownedSentinel) tag["downedSentinel"] = true;
        if (DownedDevourer) tag["downedDevourer"] = true;
        if (AbyssX > 0) { tag["abyssX"] = AbyssX; tag["abyssY"] = AbyssY; }
    }
    public override void LoadWorldData(TagCompound tag) { DownedSentinel = tag.GetBool("downedSentinel"); DownedDevourer = tag.GetBool("downedDevourer"); AbyssX = tag.GetInt("abyssX"); AbyssY = tag.GetInt("abyssY"); }
    public override void NetSend(BinaryWriter writer) { writer.Write(DownedSentinel); writer.Write(DownedDevourer); writer.Write(AbyssX); writer.Write(AbyssY); }
    public override void NetReceive(BinaryReader reader) { DownedSentinel = reader.ReadBoolean(); DownedDevourer = reader.ReadBoolean(); AbyssX = reader.ReadInt32(); AbyssY = reader.ReadInt32(); }
}
