#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ExcelDataReader;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class EditorConstantTable
{
    [MenuItem(EditorMenuNames.k_constantTable)]
    public static void Run()
    {
        ConvertExcels();

        AssetDatabase.Refresh();
    }

    private static void ConvertExcels()
    {
        // 경로 추출
        var paths = Directory.GetFiles(EditorConstantTableSettings.k_excelPath, "*.xlsx");
        
        // 생성 폴더 없을시 생성
        if (!Directory.Exists(EditorConstantTableSettings.k_assetPath))
            Directory.CreateDirectory(EditorConstantTableSettings.k_assetPath);
        
        // 경로를 순회
        foreach (string path in paths)
        {
            // 데이터 추출
            var data = ParseExcel(path);
        
            // 이름 가져오기
            var assetName = Path.GetFileNameWithoutExtension(path);
            var className = EditorConstantTableSettings.k_classNameDict.GetValueOrDefault(assetName);
            
            // 저장할 경로와 클래스 타입을 얻는다.
            var assetPath = EditorConstantTableSettings.k_assetPath + "/" + assetName + ".asset";
            var classType = Type.GetType($"{EditorConstantTableSettings.k_namespaceName}.{className}, Assembly-CSharp");
            
            // 다이나믹 형식으로 호출 한다.
            dynamic dataTable = AssetDatabase.LoadAssetAtPath(assetPath, classType);

            // 없으면 새로 생성 한다.
            if (dataTable == null)
            {
                dataTable = ScriptableObject.CreateInstance(classType);
                AssetDatabase.CreateAsset(dataTable, assetPath);

                return;
            }

            // 로딩을 시키고 나서 업데이트 한다.
            dataTable.Load(data);
            EditorUtility.SetDirty(dataTable);
        }
    }

    private static IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> ParseExcel(string excelPath)
    {
        var result = new Dictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>>();
        
        try
        {
            using var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            using var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
            });

            for (var s = 0; s < dataSet.Tables.Count; s++)
            {
                var table = dataSet.Tables[s];

                var rows = new Dictionary<int, IReadOnlyList<object>>();

                for (int r = 0; r < table.Rows.Count; r++)
                {
                    var cols = new List<object>();

                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        var value = table.Rows[r][c];

                        if (value == null)
                        {
                            break;
                        }

                        cols.Add(value);
                    }

                    rows.Add(r, cols);
                }

                result.Add(s, rows);
            }
        }

        catch (Exception e)
        {
            Debug.Assert(false, e.Message);
        }
        
        return result;
    }
}
#endif