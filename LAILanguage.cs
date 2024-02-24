using R2API;

namespace LobbyAppearanceImprovements
{
    public static class LAILanguage
    {
        //this is temporary
        public static void Init()
        {
            //Scene Titles + Subtitles
            LanguageAPI.Add("LAI_MAP_CAPTAINSHELM_TITLE", "Captain's Helm");
            LanguageAPI.Add("LAI_MAP_CAPTAINSHELM_SUBTITLE", "Stargazing Platform");
            LanguageAPI.Add("LAI_MAP_CONTACTLIGHT_TITLE", "Contact Light");
            LanguageAPI.Add("LAI_MAP_CONTACTLIGHT_SUBTITLE", "Emergency Pods");
            LanguageAPI.Add("LAI_MAP_CONTACTLIGHT_RANDOMVOIDPLANET", "Consumed Planet");
            LanguageAPI.Add("LAI_MAP_CONTACTLIGHT_RANDOMVOIDPLANET", "An Echo");
            LanguageAPI.Add("LAI_MAP_LOBBY_TITLE", "UES SAFE TRAVELS");
            LanguageAPI.Add("LAI_MAP_LOBBY_SUBTITLE", "Captain's Ship");
            LanguageAPI.Add("LAI_MAP_LOBBY_INFINITETOWER_TITLE", "U?S ??FE TR?V??S");
            LanguageAPI.Add("LAI_MAP_LOBBY_INFINITETOWER_SUBTITLE", "C?p??in's S??p");
            LanguageAPI.Add("LAI_MAP_LOBBYMULTI_TITLE", "Survivor's Room");
            LanguageAPI.Add("LAI_MAP_LOBBYMULTI_SUBTITLE", "The Multi Before The Play");
            LanguageAPI.Add("LAI_MAP_VOIDOUTROFLOOR_TITLE", "Sunk t' Nadir"); //Sunk to + lowest point of anything
            LanguageAPI.Add("LAI_MAP_VOIDOUTROFLOOR_SUBTITLE", "The Ocean Floor");
            //Scene Formatting Tokens

            LanguageAPI.Add("LAI_MAP_TITLE_FORMAT", "{0}");
            LanguageAPI.Add("LAI_MAP_SUBTTILE_FORMAT", "<color=grey>{0}</color>");
            LanguageAPI.Add("LAI_MAP_LAYOUT_FORMAT", "<color=grey>{0}</color>");

            //LAYOUTS
            LanguageAPI.Add("LAI_LAYOUT_ROR2_TITLE", "Risk of Rain, Too");
            LanguageAPI.Add("LAI_LAYOUT_PALADINONLY_TITLE", "Paladin's Entrance");
            LanguageAPI.Add("LAI_LAYOUT_CAPTAINSHELM_TITLE", "Outlook");
            LanguageAPI.Add("LAI_LAYOUT_MOONDEFAULT_TITLE", "Silence");

            LanguageAPI.Add("LAI_EMPTY_TITLE", "");
        }
    }
}