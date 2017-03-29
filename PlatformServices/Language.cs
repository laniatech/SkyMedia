namespace AzureSkyMedia.PlatformServices
{
    public static class Language
    {
        public static string GetLanguageCode(string sourceUrl)
        {
            string[] sourceInfo = sourceUrl.Split('.');
            string fileName = sourceInfo[sourceInfo.Length - 2];
            return fileName.Substring(fileName.Length - 2);
        }

        public static string GetLanguageLabel(string languageCode)
        {
            string languageLabel = string.Empty;
            switch (languageCode.Substring(0, 2).ToLower())
            {
                case "en":
                    languageLabel = "English";
                    break;
                case "es":
                    languageLabel = "Spanish";
                    break;
                case "ar":
                    languageLabel = "Arabic";
                    break;
                case "zh":
                    languageLabel = "Chinese";
                    break;
                case "fr":
                    languageLabel = "French";
                    break;
                case "de":
                    languageLabel = "German";
                    break;
                case "it":
                    languageLabel = "Italian";
                    break;
                case "ja":
                    languageLabel = "Japanese";
                    break;
                case "pt":
                    languageLabel = "Portuguese";
                    break;
            }
            return languageLabel;
        }
    }
}