using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace PandaIsPandaMvp
{
    public class Demo : MonoBehaviour
    {
        [Header("# References")]
        [SerializeField] private Board m_board;
        [SerializeField] private Round m_round;

        private void Start()
        {
            m_round.Setup();
        }

        private void OnEnable()
        {
            m_board.Init();
            m_round.Begin();
        }

        private void OnDisable()
        {

        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_board.AddItem(true);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                m_board.AddItem(false);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
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

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                var path = Path.Combine(Application.persistentDataPath, "Board.json");
                var json = File.ReadAllText(path);
                
                var data = JsonConvert.DeserializeObject<BoardData>(json);
                
                m_board.LoadData(data);
                
            }
#endif
        }
    }
}

