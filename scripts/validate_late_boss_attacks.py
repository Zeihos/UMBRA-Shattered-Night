#!/usr/bin/env python3
"""Static checks for late-game boss attacks (not in-game or multiplayer tests)."""
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
def read(path):
    return (ROOT / path).read_text(encoding="utf-8")

fx = read("Content/Systems/UMBRAVisualFX.cs")
for name, windup, dash, projectile in (
    ("RiftArchon", 24, 20, "UmbralBolt"),
    ("AstralSovereign", 26, 22, "AstralPulse"),
    ("AshenOverlord", 28, 22, "ColossusCinder"),
):
    code = read(f"Content/NPCs/{name}.cs")
    assert f"NPC.ai[3] = {windup}f;" in code, f"{name}: missing dash warning"
    assert f"NPC.ai[1] = {dash}f;" in code, f"{name}: missing dash"
    assert "NPC.ai[2] = " in code, f"{name}: missing radial charge"
    assert code.index("if (NPC.ai[2] > 0f)") < code.index("if (NPC.ai[3] > 0f)")
    assert code.index("if (NPC.ai[3] > 0f)") < code.index("if (NPC.ai[1] > 0f)")
    assert code.index("if (NPC.ai[1] > 0f)") < code.index("Vector2 hover")
    assert code.index("Vector2 hover") < code.index("if (Main.netMode == NetmodeID.MultiplayerClient)")
    assert "NPC.ai[3] <= 0f && Main.netMode != NetmodeID.MultiplayerClient" in code
    assert "NPC.ai[2] <= 0f && Main.netMode != NetmodeID.MultiplayerClient" in code
    assert code.count("Projectile.NewProjectile(") == 2, f"{name}: unexpected attack change"
    assert f"ModContent.ProjectileType<{projectile}>()" in code
    assert "NPC.netUpdate = true;" in code, f"{name}: no state sync"
    assert "!Main.dedServ" in code, f"{name}: server-side particles"
    assert f"ModContent.NPCType<{name}>()" in fx
    assert re.search(rf"npc\.type == ModContent\.NPCType<{name}>\(\).*?npc\.ai\[3\] / {windup}f", fx, re.S), f"{name}: incorrect FX duration"
assert "NPC.localAI[0]" not in read("Content/NPCs/RiftArchon.cs")
assert "lateBossDash" in fx and "DrawDashWarning(center, npc, charge, tint)" in fx
assert "npc.ai[3] > 0f" in fx
assert "if (Main.dedServ" in fx
for forbidden in ("Projectile.NewProjectile(", "NPC.NewNPC(", "NetMessage.SendData("):
    assert forbidden not in fx, f"Pure visual module creates gameplay objects: {forbidden}"
print("Late-boss dash warnings, radial phases and network authority: source-level checks passed.")
