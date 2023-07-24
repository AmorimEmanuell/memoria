# Memoria RPG
This is a puzzle based RPG game in which you defeat hordes of monsters by mastering concentration and memory skills.

Still a work in progress. You can download it here: [MemoriaRPG](../assets/Builds/memoriaRPG_0.1.apk). This is an .apk, so you'll need to install it manually in your device with adb. Alternatively you can download this repo and build yourself or try in the UnityEditor.
Current Unity Version: 2022.3.5f1 LTS

## v0.1
![CoreCombatScreen](../assets/Captures/memoriarpgv0.1core.jpg)

This is the first playable version. Very simple still, no environment background, just unity default skybox. No main menu, just the game running indefinetly.

The core gameplay consists of a memory match game. The rules are simple, you attack the enemy if you correcty match a pair of cards. But in case you fail, enemies start preparing to attack. If you fail matching pairs 3 times in a row, you receive damage depending on the enemy stats. The cards assets were taken from: [Simple Medieval Signs](https://assetstore.unity.com/packages/2d/environments/simple-medieval-signs-62229).

![Match Success](../assets/Captures/memoriarpgv0.1matchsuccess.jpg)
![Match Fail](../assets/Captures/memoriarpgv0.1matchfail.jpg)

You can heal yourself when in low health by clicking on the potion button. There's a 10% chance that enemies will drop potions when dying. But you have to be quick as these potions will not remain on the screen forever. The potion asset used can be found at: [RPG inventory icons](https://assetstore.unity.com/packages/2d/gui/icons/rpg-inventory-icons-56687).

![Enemy dropping potions](../assets/Captures/memoriarpgv0.1enemydead.jpg)

There are two types of enemies. Each enemy have 3 different variations of stats.

![Slime](../assets/Captures/memoriarpgv0.1enemy1.jpg)
![Turtle Shell](../assets/Captures/memoriarpgv0.1enemy2.jpg)

These enemy models are free from the unity assets store: [RPG Monster Duo PBR Polyart](https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-duo-pbr-polyart-157762).

In case of defeat there is a game over screen that allows you to retry. The assets for this window can be found at: [Fantasy Wooden GUI : Free](https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811).

![Game Over](../assets/Captures/memoriarpgv0.1gameover.jpg)


