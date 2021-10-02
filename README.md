
# Lobby Appearance Improvements
<p align="center">
<img src="https://raw.githubusercontent.com/DestroyedClone/LobbyAppearanceImprovements/master/readme/scenes%20collage.webp" alt="Banner" width="1000" height="400"/>
</p>

This mod contains multiple settings which change the look, scene, and even character layout of the lobby.
This mod is likely incompatible with other mods that modify the lobby scene itself, as such mods like ScrollableLobbyUI are compatible.

## Discord

Because there's no comments on thunderstore, made a discord. Plug any comments, complaints, suggestions in the appropriate channels. Click on Artificer's dumb face to go to it.

[![](https://media.discordapp.net/attachments/891016031315845202/893963899769086003/fatass.png)](https://discord.gg/DpHu3qXMHK "Discord Link")

## To Do / Known Issues
* More support for unsupported characters
* More character layouts
* Live updates for loadout changes
* Restore additional settings for SurvivorsInLobby

## Default (For Reference)
This was taken with KingEnderBrine's [ScrollableLobbyUI](https://thunderstore.io/package/KingEnderBrine/ScrollableLobbyUI/) and [RandomCharacterSelection](https://thunderstore.io/package/KingEnderBrine/RandomCharacterSelection/).

<p align="center">
<img src="https://i.imgur.com/7XRPDYu.png" alt="Slightly modded lobby" width="800" height="400"/>
</p>

## Color, Intensity, Flicker

| #ff0000 | #ffff00 | #00ff00 | #00ffff | #0000ff | #ff00ff | #ffffff | #000000 |
|---------|---------|---------|---------|---------|---------|---------|---------|
| ![red](https://i.imgur.com/ezcmCIC.png)     | ![yellow](https://i.imgur.com/2wKoae8.png)  | ![green](https://i.imgur.com/wdwHQsU.png)   | ![cyan](https://i.imgur.com/vTlBzp8.png)    | ![blue](https://i.imgur.com/YII0gYj.png)    | ![purple](https://i.imgur.com/ywPgFT4.png)  | ![white](https://i.imgur.com/yhtgkg5.png)   | ![black](https://i.imgur.com/2heSySe.png)   |

Or look up "HEX Color Picker".

***Intensity***

| 5 | 10 | 50 | 100 |
|:--:|:--:|:--:|:--:|
| <img src="https://i.imgur.com/gk8r70g.png" alt="5x intensity" width="75%"/> | <img src="https://i.imgur.com/6DsFwWc.png" alt="10x intensity" width="75%"/> | <img src="https://i.imgur.com/j0yBdN9.png" alt="50x intensity" width="75%"/> | <img src="https://i.imgur.com/av0slmz.png" alt="100x intensity" width="75%"/> |

## UI Blur, Fade, Scale

|0 Blur| Fade |
|--|--|
| ![Blur](https://i.imgur.com/TYYebOe.png) | <img src="https://i.imgur.com/jFGlDe7.png" alt="drawing" height="300"/> 
| 75% Scale | 50% Scale |
| ![UI 75% Scale](https://i.imgur.com/V6c1Ze2.png) | ![UI 50% Scale](https://i.imgur.com/s8PJzYA.png) |

## Post Processing
There's a murky post-processing effect that can be disabled.

<p align="center">
<img src="https://i.imgur.com/c9vMtVp.png" alt="post-processing" width="50%" height="75%"/>
</p>

## Pad Scale, Props, Shaking

You can resize the size of the character pads to make the background easier to see. The props can be disabled, and so can the shaking.

## Scenes
Newly added with version 1.1.0, now you can set the background of the lobby to a different look. 

## Character Layouts
Previously known as "Survivors In Lobby", this shows survivors in the background at preset locations and may add new things to the existing scene.

Some layouts are locked behind mods being loaded, and all scenes and layouts are case-sensitive.

## Shadow Locked Characters
Configuration option included that darkens all characters that are locked. May not work if the character is spawned outside of the regular method. Does not include characters selected by other players.

![enter image description here](https://raw.githubusercontent.com/DestroyedClone/LobbyAppearanceImprovements/master/readme/notunlocked.webp)

## Changing Scenes+Layouts
<p align="center">
<img src="https://raw.githubusercontent.com/DestroyedClone/LobbyAppearanceImprovements/master/readme/tutorial.webp" alt="post-processing" width="50%" height="75%"/>
</p>

1. Click on the Wrench icon, with four lines behind it, to open the Mods configuration menu provided by InLobbyConfig.
2. Expand the "Lobby Appearance Improvements" config menu.
3. Expand the "[Scenes+Layouts]" config category.
4. Set the name of your scene in "Select Scene"
5. Set the name of your layout in "Character Layout Name"
6. Check the "Confirm Choice".
	7. If you are updating your choice, you'll need to set Confirm Choice to false before setting it to true.

## List of Scenes w/ Layouts
A blank layout exists with no background survivors: `Any_Empty`. If a layout has a survivor name linked before it, that means that the scene is locked to that survivor and will not load unless you also have that survivor installed.

- Lobby (Default)
	- Lobby_DestroyedClone_Default
	- Lobby_DestroyedClone_Original
- Arena
	- Arena_DestroyedClone_Nemesis
- ArtifactWorld
- BlackBeach
- CaptainsHelm
- DampCave
- FoggySwamp
- FrozenWall
- GoldShores
- GolemPlains
- GooLake
- Limbo
- Moon
	- Moon_DestroyedClone_Default
	- [♘Paladin♘](https://thunderstore.io/package/Paladin_Alliance/PaladinMod/) Moon_DestroyedClone_PaladinOnly
- MysterySpace
- RootJungle
- ShipGraveyard
- SkyMeadow
- WispGraveyard


Scene: Lobby, Layout: Lobby_DestroyedClone_Default
![Preview](https://i.imgur.com/RDFTLOa.png)

## Changelog
* `v1.1.0` - Code overhaul
	* Fixed Blur unable to be set to 0.
	* Added custom scenes (17)
	* Added custom layouts (5)
	* Added InLobbyConfig hard dependency
	* Added parallax option
	* Added option that shades the characters darker if they are locked,
* `v1.0.1` - Fixed readme?, lobby light color now remains unchanged if set to "default", code cleanup, added option to replay anim on select
* `v1.0.0` - Release

## Credits

Rico - Icon
> Written with [StackEdit](https://stackedit.io/).
