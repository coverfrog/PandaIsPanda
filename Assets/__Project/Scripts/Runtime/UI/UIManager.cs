using System;
using UnityEngine;

namespace PandaIsPanda
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Transform m_trCanvas;
        
        public static UIManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public UIPage GetPage(UIPageType type)
        {
            var adr = type.ToAddress();
            var ins = AddressableUtil.Instantiate<UIPage>(adr, true, m_trCanvas);
            
            ins.transform.SetAsLastSibling();
            
            return ins;
        }
        
        public T GetPage<T>(UIPageType type) where T : UIPage
        {
            var adr = type.ToAddress();
            var ins = AddressableUtil.Instantiate<T>(adr, true, m_trCanvas);
            
            ins.transform.SetAsLastSibling();
            
            return ins;
        }
    }
}