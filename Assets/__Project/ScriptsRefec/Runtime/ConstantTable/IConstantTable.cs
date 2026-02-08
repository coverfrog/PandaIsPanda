using System.Collections.Generic;

namespace PandaIsPanda
{
    public interface IConstantTable
    {
        void Load(IReadOnlyDictionary<int, IReadOnlyDictionary<int, IReadOnlyList<object>>> data);
    }
}