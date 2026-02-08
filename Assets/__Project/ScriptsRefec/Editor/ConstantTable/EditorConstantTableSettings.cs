using System.Collections.Generic;

#if UNITY_EDITOR
public static class EditorConstantTableSettings
{
    public const string k_excelPath = "Assets/__Project/Excel/ConstantTable";
    
    public const string k_assetPath = "Assets/__Project/Addressable/ConstantTable";

    public const string k_namespaceName = "PandaIsPanda";

    public static readonly Dictionary<string, string> k_classNameDict = new()
    {
        { "Round", "RoundConstantTable"},
        { "Unit", "UnitConstantTable"},
    };
}
#endif