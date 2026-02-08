using System;
using UnityEngine;

namespace PandaIsPanda
{
    public class Monster : MonoBehaviour
    {
        private GameObject m_g0;
        private GameObject m_g1;
        private GameObject m_g2;
        private GameObject m_g3;
        
        private RectTransform m_rt;

        private int m_corner = 1;

        private float m_hp = 100;
        
        private void Start()
        {
            m_rt = GetComponent<RectTransform>();
            
            m_g0 = GameObject.Find("[ Cell ][ 0 x 0 ]");
            m_g1 = GameObject.Find("[ Cell ][ 6 x 0 ]");
            m_g2 = GameObject.Find("[ Cell ][ 6 x 6 ]");
            m_g3 = GameObject.Find("[ Cell ][ 0 x 6 ]");
            
            var s= RectTransformUtility.WorldToScreenPoint(null, m_g0.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("UI Board").GetComponent<RectTransform>(), s, null, out var local);
            
            m_rt.anchoredPosition = local;
        }

        private void Update()
        {
            GameObject[] os = new GameObject[] { m_g0, m_g1, m_g2, m_g3 };

            var target = os[m_corner].transform.position;
            
            if (Vector3.Distance(m_rt.transform.position, target) > 1.0f)
            {
                m_rt.transform.position += (target - m_rt.transform.position).normalized * (150.0f * Time.deltaTime);
            }

            else
            {
                m_corner = (m_corner + 1) % os.Length;
            }
        }

        public void Attack(int damage)
        {
            m_hp -= damage;
            if (m_hp <= 0)
                Destroy(gameObject);
            
            Debug.Log(m_hp);
        }
    }
}