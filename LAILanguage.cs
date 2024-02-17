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

            //Scene Formatting Tokens

            LanguageAPI.Add("LAI_MAP_TITLE_FORMAT", "{0}");
            LanguageAPI.Add("LAI_MAP_SUBTTILE_FORMAT", "<color=grey>{0}</color>");
        }
    }
}