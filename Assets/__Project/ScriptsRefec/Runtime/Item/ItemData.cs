using System;

namespace PandaIsPanda
{
    [Serializable]
    public class ItemData
    {
        public ItemConstant Constant { get; }
        
        public int Count { get; private set; }
        
        public ItemData(ItemConstant constant, int count)
        {
            Constant = constant;
            Count = count;
        }
    }
}