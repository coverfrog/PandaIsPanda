namespace PandaIsPanda
{
    public static class ItemExtensionMethod
    {
        public static ItemData ToItemData(this ulong id, int count)
        {
            ItemConstant cons = DataManager.Instance.ItemConstants[id];
            ItemData data = new ItemData(cons, count);
            
            return data;
        }
    }
}