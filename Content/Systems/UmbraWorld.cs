using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace UmbraShatteredNight.Content.Systems;
public class UmbraWorld : ModSystem
{
    public static bool DownedSentinel;
    public override void OnWorldLoad() => DownedSentinel = false;
    public override void OnWorldUnload() => DownedSentinel = false;
    public override void SaveWorldData(TagCompound tag) {
        if (DownedSentinel) tag["downedSentinel"] = true;
    }
    public override void LoadWorldData(TagCompound tag) => DownedSentinel = tag.GetBool("downedSentinel");
    public override void NetSend(BinaryWriter writer) => writer.Write(DownedSentinel);
    public override void NetReceive(BinaryReader reader) => DownedSentinel = reader.ReadBoolean();
}
