using System;

namespace PandaIsPanda
{
    [Serializable]
    public class CountValue<T>
    {
        public T value;
        public int count;
    }
}