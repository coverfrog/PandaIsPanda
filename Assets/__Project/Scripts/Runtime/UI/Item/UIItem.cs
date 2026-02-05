using UnityEngine;
using UnityEngine.UI;

namespace PandaIsPanda
{
    public class UIItem : MonoBehaviour
    {
        [Header("# References")] 
        [SerializeField] private Image m_imgIcon;
        
        public UIItem SetEnableIcon(bool enable)
        {
            if (m_imgIcon) m_imgIcon.enabled = enable;
            return this;
        }
        
        public UIItem SetItem(Item item)
        {
            return item != null ? 
                SetItem_Exists(item) : 
                SetItem_Null();
        }
        
        private UIItem SetItem_Exists(Item item)
        {
            if (m_imgIcon)
            {
                m_imgIcon.sprite = item.Constant.Icon;
            }
            return this;
        }

        private UIItem SetItem_Null()
        {
            if (m_imgIcon)
            {
                m_imgIcon.sprite = null;
            }
            return this;
        }
    }
}