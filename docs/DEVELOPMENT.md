# Разработка UMBRA

[← Вернуться на главную](../README.md)

Исходный код написан на **C# для tModLoader**. Основной идентификатор мода — `UmbraShatteredNight`. В CI зафиксирована версия **tModLoader v2026.09.3.1** и установлен **.NET 8**.

## Как устроен код

| Путь | Что здесь находится |
|---|---|
| `Content/NPCs/` | NPC, боссы, их AI и визуальные кадры |
| `Content/Items/` | Предметы, оружие, рецепты, доспехи |
| `Content/Projectiles/` | Призываемые существа и снаряды |
| `Content/Biomes/` | Активация биомов |
| `Content/Systems/` | Мирогенерация, мировая прогрессия, события, VFX |
| `Content/Tiles/` | Блоки и руды |
| `Content/Buffs/` | Баффы |
| `Localization/` | Переводы `ru-RU` и `en-US` |
| `scripts/` | Проверки целостности ресурсов и исходников |

## Локальная проверка

Команды запускаются из **корня репозитория**. Для этих проверок достаточно Python, установленного в системе.

```bash
python scripts/validate_source.py
python scripts/validate_visual_animation.py
python scripts/validate_combat_visuals.py
python scripts/validate_biome_atmosphere.py
python scripts/validate_invasion_spawn.py
python scripts/validate_safe_spawns.py
python scripts/validate_repository.py
```

Они обнаруживают часть ошибок в ссылках на классы, изображениях, анимациях и локализации. **Они не заменяют компиляцию C# и тестирование в Terraria.**

## Сборка

В локальном tModLoader папка проекта должна называться `UmbraShatteredNight` и располагаться внутри `ModSources`. Сборку запускают через **Workshop → Develop Mods → Build + Reload**.

В GitHub предусмотрены два workflow:

1. [**UMBRA source integrity**](https://github.com/Zeihos/UMBRA-Shattered-Night/actions/workflows/static-checks.yml) — проверяет исходники и ресурсы.
2. [**UMBRA tModLoader build**](https://github.com/Zeihos/UMBRA-Shattered-Night/actions/workflows/tmodloader-build.yml) — компилирует мод и сохраняет `.tmod` как артефакт `UMBRA-built-tmod`.

Скрипт повторной автоматической перекраски графики удалён из активной ветки: он мог бы перезаписать уже утверждённые спрайты. Изменения графики вносятся вручную, через отдельные коммиты и проверку.

## Правила изменений

- Не добавляй в Git `.tmod`, `bin/`, `obj/` и кэш.
- При изменении предмета, NPC или снаряда проверь наличие и корректность PNG.
- Новые игровые названия добавляй в обе локализации.
- Новую графику и изменения анимации показывай на скриншотах или в GIF в pull request.
- Если затронуты мирогенерация, сетевой код, сохранения или баланс, указывай, что **проверено в игре**, а что — только статически.
- Не меняй историю `main` с принудительным push.

Как предложить исправление — в [CONTRIBUTING.md](../CONTRIBUTING.md).
