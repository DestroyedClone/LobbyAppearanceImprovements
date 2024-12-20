﻿* v1.3.0
	* Fixed for Seekers of the Storm
	* Scenes
		➕ Added 11 new scenes:
			* ArtifactWorld01
			* ArtifactWorld02
			* ArtifactWorld03
			* Habitat
			* HabitatFall
			* HelminthRoost
			* LakesNight
			* LemurianTemple
			* Meridian
			* Village
			* VillageNight
	* Header and Footer text now are centered relative to the character select UI instead of relative to the screen.
	* Config
		* Shaking config now only affects Lobby and name is changed to reflect that.
	* RoR2 Patch
		* Fixed user setting screen shake scale not working in lobby.
	* Some unneeded logging has been removed.
* v1.2.?
	* Scenes
		* ➕ Added new music option: auto, default, and defined
			* auto chooses a pre-defined theme for the Scene
			* default will play the default music or any other existing music overrides
			* defined as in if you put 'muFULLSong02' it will play its respective associated track.
		* ➕Added 4 new scenes: 
			* codes (Artifact Code event)
			* lakes (Verdant Lakes)
			* eclipse (CoolerEclipseLobby, permission given by Nuxlar),
			* rescueship (Moon Escape)
		* ➕Added new event for scenes to have something activated upon the vote starting.
		* 🔧Updated some scenes
			* AncientLoft: Changed pos/rot
			* Arena: Portal is changed to OnVoteStarted
			* ArtifactWorld: Portal is changed to OnVoteStarted
			* Goldshores: Added beacon, added OnVoteStarted effect
		* ➕UI now has an option to show Seer text for the scene below the Ready button.
		* 🔧Disabling shaking config should now properly disable the lobby shaking. Probably won't.
		* 🔧Added fix to possibly add mod compat with DropInMultiplayer GUI
		* 🔧Post processing should disable post processing when other stages have them, instead of just the lobby.
		* 🔧Background Elements is now renamed to "Scene: Lobby" and put below Scenes+Layouts
		* 🔧Scenes use the background material from their respective stages.
		* ➕Added French Translation (Thanks, CobaltKitsune!)
* v1.2.0
	* Config
		* ➕ Added Scene Header: Showing the scene title, subtitle, and layout title.
		* 🔧Fixed BlurOpacity not correctly parsing partial values
		* 🔧Fixed Disable Shaking not disabling after a scene load
		* 🔧Selected scenes are now lowercase only
		* 🔧Added identifier to some config values that only work in Lobby scene
		* ➕Added reset camera key (default '=')
		* 🔧Light color inlobbyconfig now has a color display
		* 🔧Renamed config descriptions for clarity.
	* Language
		* ➕Added new English tokens for the new headers
	* Scenes
		* ➕Added 8 new scenes: AncientLoft, Lobby, LobbyVoid, SnowyForest, SulfurPools, VoidOceanFloor, VoidRaid, VoidStage
		* ➕Skybox now changes per scene
	* Layouts
		* 🔧Moon_Default now has a Mithrix Constellation from itmoon, and a dead commando prop
		* ➕Added new layouts: Lobby_ROR2, Moon_Default
		*  🔧 Removed some layouts temporarily due to internal code changes
	* Other
		* 🔧Various internal fixes and code reorganization and cleanup.
* `v1.1.1`
	* Added config category "Character Pad"
		* Character Pad Scale goes here instead of Background
		* Added Character Turning - click to drag to rotate your character
		* Character Turning Multiplier - increases the speed at which the character is turned
	* Clamped UI Scale between 0.5 and 1.75 to prevent the config setting from becoming inaccessible.
	* Updated console commands to no longer have server-side flags
	* Layouts
		* Renamed all layouts from "`[SceneName]_[Author]_[LayoutName]`" to "`[SceneName]_[LayoutName]`"
		* Added new layouts (2)
			* BlackBeach_League, WispGraveyard_Snipers
		* Restored option to zoom in on character selected.
		* Added CharacterCameraSetting for layouts to include their own camera settings.
		* Added some missing character camera settings to the layouts.
* `v1.1.0` - Code overhaul
	* Fixed Blur unable to be set to 0.
	* Added custom scenes (17)
	* Added custom layouts (5)
	* Added InLobbyConfig hard dependency
	* Added parallax option
	* Added option that shades the characters darker if they are locked,
* `v1.0.1` - Fixed readme?, lobby light color now remains unchanged if set to "default", code cleanup, added option to replay anim on select
* `v1.0.0` - Release


> Written with [StackEdit](https://stackedit.io/).