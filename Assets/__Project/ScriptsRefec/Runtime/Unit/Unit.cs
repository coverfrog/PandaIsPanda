using UnityEngine;

namespace PandaIsPanda
{
    public class Unit : MonoBehaviour
    {
        public void Setup(UnitData unitData)
        {
           LogUtil.Log(DataManager.Instance.LocalizationTextConstants[unitData.Constant.NameId].Kr);
        }
    }
}