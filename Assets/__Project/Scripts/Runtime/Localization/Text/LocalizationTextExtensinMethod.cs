namespace PandaIsPanda
{
    public static class LocalizationTextExtensinMethod
    {
        public static string ToLocalizationText(this ulong id)
        {
            string str = "";
            
            if (!DataManager.Instance.LocalizationTextConstants.TryGetValue(id, out var texts))
            {
                return str;
            }
            
            var nation = DataManager.Instance.LocalizationNation;

            str = nation switch
            {
                LocalizationNation.Kr => texts.Kr,
                LocalizationNation.En => texts.En,
                LocalizationNation.Fr => texts.Fr,
                _ => ""
            };

            return str;
        }
    }
}