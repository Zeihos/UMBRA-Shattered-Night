using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;

namespace UmbraShatteredNight.Content.Commands;

/// <summary>Reports the next reachable UMBRA progression step for this world.</summary>
public sealed class UmbraGuideCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;
    public override string Command => "umbraguide";
    public override string Usage => "/umbraguide";
    public override string Description => "Показывает следующую цель UMBRA / Shows the next UMBRA goal";

    private static string Text(string key) =>
        Language.GetTextValue("Mods.UmbraShatteredNight.Guide." + key);

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        caller.Reply(Text("Title"), new Color(174, 112, 239));

        // Prioritize the live event over the next boss so the guidance stays
        // accurate after saving/reloading an invasion in single- or multiplayer.
        if (UmbraWorld.NightInvasionActive)
        {
            caller.Reply(Language.GetTextValue(
                "Mods.UmbraShatteredNight.Guide.InvasionActive",
                UmbraWorld.NightInvasionWave, UmbraWorld.NightInvasionKills));
        }
        else if (!UmbraWorld.DownedSentinel)
            caller.Reply(Text("Sentinel"));
        else if (!UmbraWorld.DownedNightInvasion)
            caller.Reply(Text("Invasion"));
        else if (!UmbraWorld.DownedArchon)
            caller.Reply(Text("Archon"));
        else if (!UmbraWorld.DownedAstralHeart)
            caller.Reply(Text("AstralHeart"));
        else if (!UmbraWorld.DownedAstralSovereign)
            caller.Reply(Text("Sovereign"));
        else if (!UmbraWorld.DownedAshenColossus)
            caller.Reply(Text("Colossus"));
        else if (!UmbraWorld.DownedAshenOverlord)
            caller.Reply(Text("Overlord"));
        else
            caller.Reply(Text("Completed"));

        // The Shard Devourer has its own optional progression branch.
        if (!UmbraWorld.DownedDevourer && !UmbraWorld.NightInvasionActive)
            caller.Reply(Text("OptionalDevourer"), new Color(170, 154, 204));
    }
}
