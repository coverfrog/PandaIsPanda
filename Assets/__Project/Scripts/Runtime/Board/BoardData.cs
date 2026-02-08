using System;

namespace PandaIsPandaMvp
{
    [Serializable]
    public struct BoardData
    {
        public ulong[,] grid;
        public int selectionRow;
        public int selectionColumn;
    }
}