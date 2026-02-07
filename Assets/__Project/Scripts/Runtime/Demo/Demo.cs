using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace PandaIsPanda
{
    public class Demo : MonoBehaviour
    {
        private Board m_board;

        private void Awake()
        {
            m_board = FindAnyObjectByType<Board>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_board.AddItem();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                var data = m_board.ToData();
#if false

                var msg = "Grid\n";
                var grid = data.grid;
                
                int rowLength = grid.GetLength(0);
                int columnLength = grid.GetLength(1);
                
                for (int r = 0; r < rowLength; r++)
                {
                    for (int c = 0; c < columnLength; c++)
                    {
                        var id = grid[r, c];
                        msg += $"{id} ";
                    }
                    msg += "\n";
                }
                
                msg += "\n";
                msg += "Selection \n";
                msg += $"Row: {data.selectionRow}, Column: {data.selectionColumn}\n";
                
                Debug.Log(msg);
#endif
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                var path = Path.Combine(Application.persistentDataPath, "Board.json");
                
                File.WriteAllText(path, json);
                
                Debug.Log(path);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                var path = Path.Combine(Application.persistentDataPath, "Board.json");
                var json = File.ReadAllText(path);
                
                var data = JsonConvert.DeserializeObject<BoardData>(json);
                
                m_board.LoadData(data);
                
            }
#endif
        }

        private void OnEnable()
        {
            m_board.Init();
        }

        private void OnDisable()
        {
            
        }
    }
}

