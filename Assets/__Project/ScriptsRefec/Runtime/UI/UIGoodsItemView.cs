using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PandaIsPanda
{
    public class UIGoodsItemView : MonoBehaviour
    {
        [Header("# Option")] 
        [SerializeField] private ulong m_inventoryId;
        [SerializeField] private ulong m_itemId;
        
        [Header("# References")]
        [SerializeField] private Image m_imgIcon;
        [SerializeField] private RTLTextMeshPro m_txtCount;
    }
}