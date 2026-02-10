namespace PandaIsPanda
{
    public enum UIPageType
    {
        GameStory
    }

    public static class UIPageTypeExtensions
    {
        public static string ToAddress(this UIPageType type) => type switch
        {
            UIPageType.GameStory => "uipage/gamestory",
            _ => ""
        };
    }
}