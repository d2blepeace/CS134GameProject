# Roll-A-Ball Adventure

A 3D action game built in Unity where you control a rolling ball, dodge enemy projectiles, and parry them back to defeat enemies — all while collecting points to complete the level

## Gameplay Overview

The player navigates a ball through levels populated with patrolling enemies and collectible pickups. Enemies can detect the player, chase, and fire projectiles. The core mechanic is a timed **parry**, which is pressing the parry button at the right moment to reflect incoming projectiles back toward the enemy that fired them, dealing damage and destroying them. Collect all pickups in a level to win and progress to the next.

## Controls

- Move : WASD 
- Look : Mouse 
- Jump : Space 
- Parry : R (mapped via Input System) |
- Pause : Escape 
- How to Play : H 

## Core Systems

### Player

- **Movement & Camera**: Third-person orbit camera with collision handling. The player ball moves relative to the camera's facing direction, with configurable speed, jump force, and mouse sensitivity.
- **Health** : 3 HP by default (mutable in the Inspector). Taking damage from enemy contact or failed to parry projectiles reduces health. At zero health the player dies.
- **Parry**: A short timed window activates a trigger collider (hitbox area) around the player. Any projectile entering the trigger during this window is reflected back toward the enemy that fired it, using aim-locked direction tracking. A cooldown prevents spam.

### Enemy AI

- **Patrol -> Chase -> Attack** state machine driven by sight and attack range checks.
- Enemies use Unity's NavMesh for pathfinding during patrol and chase.
- When in attack range, enemies stop, face the player, and fire projectiles at a configurable rate.
- Each enemy has its own health pool (`maxHealth` exposed in the Inspector). Reflected projectiles deal damage; at zero health the enemy is destroyed.
- An alert sound plays when the player first enters an enemy's sight range.

### Projectile

- Fires toward the player at a set speed with a limited lifetime.
- Tracks an immutable `shooter` reference (for self-collision guarding) and a mutable `Owner` reference (retained through reflection for aim-locking).
- On reflection, recalculates velocity toward the enemy's current position.
- Damages the player on hit (before parry) or the enemy on hit (after parry), then self-destructs.

### Collectibles

Rotating pickup objects tagged `PickUp`. Collecting all of them triggers the win condition for the current level.

## UI & Menus

- **Main Menu** - New Game, Resume (via PlayerPrefs save system), Settings, Tutorial, Quit.
- **Pause Menu** - Resume, Settings (mouse sensitivity), Return to Main Menu, Quit. Freezes time and unlocks the cursor.
- **How to Play** - Toggle overlay (H key) with controls reference.
- **Game Over** - Retry (reload scene) or return to Main Menu.
- **You Win** - Shown when all pickups are collected. Offers Next Level (for Level 1 and 2) or Return to Main Menu.
- **Button SFX** - Hover and click sounds on all UI buttons.

## Level Progression

Three levels with save/resume support:

1. **Level_1** - unlocks Level_2 on completion
2. **Level_2** - unlocks Final_level on completion
3. **Level_3** - clears save data on completion

Progress is persisted via `PlayerPrefs` so players can resume from the main menu.

## Project Structure

```
Assets/Scripts/
├── PlayerController.cs    # Movement, jumping, pickup collection
├── PlayerParry.cs         # Parry window, cooldown, trigger management
├── PlayerHealth.cs        # HP tracking, damage, death, fall death
├── EnemyAI.cs             # Patrol/chase/attack FSM, health, NavMesh
├── Projectile.cs          # Firing, reflection, collision damage
├── CameraController.cs    # Third-person orbit camera with collision
├── Rotator.cs             # Spinning animation for collectibles
├── MainMenuUI.cs          # Main menu navigation and save/resume
├── PauseUI.cs             # Pause/settings menu
├── HowToPlayUI.cs         # Controls overlay
├── GameOverUI.cs          # Death screen
├── YouWinUI.cs            # Win screen and level progression
├── LevelMusic.cs          # Level background music playback
├── MainMenuMusic.cs       # Main menu volume slider
├── GameAudioSetting.cs    # In-game music volume and mute toggle
├── DynamicBoxAudio.cs     # Impact SFX on player-box collisions
├── UIButtonSFX.cs         # Hover/click sounds on buttons
└── sceneLoader.cs         # Generic scene loading utility
```

## Technical Details

- **Engine:** Unity (C#)
- **Editor:** Unity 2022.3.62f3
- **Input:** Unity Input System
- **Navigation:** Unity NavMesh
- **UI:** TextMeshPro + Unity UI 
- **Audio:** Per-component AudioSource/AudioClip pairs for granular control
- **Architecture:** Component-based with clear separation of concerns - dedicated health components, split identity/ownership fields on projectiles, and serialized fields for designer-friendly tuning.

## Known issues & Future Plans
- Add more varieties of enemies with different attack pattern and movement
- More health pickup and collectibles
- Enemies sometimes get stuck with wall, need improving.
- More visual GUI: health bar, experience bar
- More levels

## Credits

- Music & SFX: free assets from freesound.org and pixabay.com
- Textures: free assets from cc0-textures.com
