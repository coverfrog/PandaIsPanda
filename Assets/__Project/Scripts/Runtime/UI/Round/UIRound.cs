using RTLTMPro;
using UnityEngine;

namespace PandaIsPandaMvp
{
    public class UIRound : MonoBehaviour
    {
        [SerializeField] private RTLTextMeshPro m_txtTimer;
        [SerializeField] private RTLTextMeshPro m_txtLive;

        public void SetTimer(float min, float sec)
        {
            m_txtTimer.text = $"{min:00}:{sec:00}";
        }
    }
}