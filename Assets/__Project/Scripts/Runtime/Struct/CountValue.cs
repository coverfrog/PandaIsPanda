using System;

namespace PandaIsPandaMvp
{
    [Serializable]
    public class CountValue<T>
    {
        public T value;
        public int count;
    }
}