using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PandaIsPanda
{
    public class UIItem : MonoBehaviour
    {
        [Header("# References")] 
        [SerializeField] private Image m_imgIcon;
        [SerializeField] private RTLTextMeshPro m_txtSubject;
        [SerializeField] private RTLTextMeshPro m_txtDescription;
        
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
                m_imgIcon.sprite = item.Constant.Icon;
            if (m_txtSubject)
                m_txtSubject.text = item.Constant.Subject;
            if (m_txtDescription)
                m_txtDescription.text = item.Constant.Description;
            return this;
        }

        private UIItem SetItem_Null()
        {
            if (m_imgIcon)
                m_imgIcon.sprite = null;
            if (m_txtSubject)
                m_txtSubject.text = "";
            if (m_txtDescription)
                m_txtDescription.text = "";
            return this;
        }
    }
}