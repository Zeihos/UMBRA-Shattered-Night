using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.Commands;
public class UmbraStatusCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;
    public override string Command => "umbrastatus";
    public override string Usage => "/umbrastatus";
    public override string Description => "Displays current UMBRA world progression";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        caller.Reply("UMBRA — World Progress", new Color(174, 112, 239));
        caller.Reply("Eclipse Sentinel: " + (UmbraWorld.DownedSentinel ? "defeated" : "not defeated"));
        caller.Reply("Shard Devourer: " + (UmbraWorld.DownedDevourer ? "defeated" : "not defeated"));
        caller.Reply(UmbraWorld.AbyssX > 0 ? "Fractured Abyss: generated" : "Fractured Abyss: not generated in this world");
    }
}
