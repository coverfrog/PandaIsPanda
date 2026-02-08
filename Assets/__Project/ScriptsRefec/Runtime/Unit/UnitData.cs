using System;

namespace PandaIsPanda
{
    [Serializable]
    public class UnitData
    {
        public UnitConstant Constant { get; }

        public UnitData(UnitConstant constant)
        {
            Constant = constant;
        }
    }
}