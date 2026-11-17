#!/usr/bin/env python3
"""Static round-robin spawn checks (not a multiplayer simulation)."""
import re
from pathlib import Path
root=Path(__file__).resolve().parents[1]
code=(root/'Content/Systems/NightInvasionSystem.cs').read_text(encoding='utf-8')
for token in ('private static int nextPlayerIndex;','nextPlayerIndex = 0;',
              '(nextPlayerIndex + offset) % Main.maxPlayers',
              '!candidate.active || candidate.dead || !candidate.ZoneOverworldHeight',
              'nextPlayerIndex = (index + 1) % Main.maxPlayers;',
              'SpawnInvasionNPC(','Main.netMode == NetmodeID.MultiplayerClient'):
    assert token in code, f'Invasion spawn regression: {token}'
assert re.search(r'^version\s*=\s*\d+\.\d+\.\d+\s*$',(root/'build.txt').read_text(encoding='utf-8'),re.M)
print('Round-robin invasion checks passed')
