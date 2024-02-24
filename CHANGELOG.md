* v1.2.0
	* CharSceneLayout: Added Lobby_ROR2
	* Config
		* Fixed BlurOpacity either being completely invisible or default
		* Added Scene Header showing the Scene's Title and Subtitle as header
		* Selected scenes are now case-insensitive only
	* Language
		* Added new English tokens for the new headers
	* Scenes
		* Added 4 new scenes: AncientLoft, Lobby, LobbyInfiniteTower, SnowyForest, SulfurPools, VoidRaid, VoidStage
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