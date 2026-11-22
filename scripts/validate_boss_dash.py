#!/usr/bin/env python3
"""Static checks for readable first-tier boss dash attacks in UMBRA 0.74.

Source-level only: no simulated Terraria ticks or real multiplayer client.
"""
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
def source(path):
    return (ROOT / path).read_text(encoding="utf-8")

fx = source("Content/Systems/UMBRAVisualFX.cs")
for name, windup, dash in (("EclipseSentinel", 30, 18), ("ShardDevourer", 26, 16)):
    code = source(f"Content/NPCs/{name}.cs")
    assert f"NPC.ai[2] = {windup}f;" in code, f"{name}: dash windup missing"
    assert f"NPC.ai[1] = {dash}f;" in code, f"{name}: dash duration missing"
    assert "NPC.ai[2]--;" in code and "NPC.ai[1]--;" in code
    charge = code.index("if (NPC.ai[2] > 0f)")
    dash_state = code.index("if (NPC.ai[1] > 0f)")
    chase = code.index("Vector2 hover") # after both attack states
    server = code.index("if (Main.netMode == NetmodeID.MultiplayerClient)")
    assert charge < dash_state < chase < server
    assert "NPC.velocity *= 0.99" in code, f"{name}: dash speed not preserved"
    assert code.index("NPC.ai[2] =") > server, f"{name}: client must not begin a dash"
    assert "NPC.ai[2] <= 0f && Main.netMode != NetmodeID.MultiplayerClient" in code
    assert code.index("Projectile.NewProjectile(") > server, f"{name}: client created a projectile"
    assert "NPC.netUpdate = true;" in code and "!Main.dedServ" in code
    assert f"ModContent.NPCType<{name}>()" in fx
    assert re.search(rf"ModContent.NPCType<{name}>\(\).*?npc.ai\[2\] / {windup}f", fx, re.S)
assert "DrawDashWarning(center, npc, charge, tint)" in fx
assert "target.Center - npc.Center" in fx and "TextureAssets.MagicPixel.Value" in fx
assert "if (Main.dedServ" in fx
for token in ("Projectile.NewProjectile(", "NPC.NewNPC(", "NetMessage.SendData("):
    assert token not in fx, f"Visual drawing should never change gameplay: {token}"
print("Eclipse Sentinel and Shard Devourer dash windups and client-only visuals: static checks passed.")
