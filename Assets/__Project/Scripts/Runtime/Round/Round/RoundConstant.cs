using System;
using UnityEngine;

namespace PandaIsPandaMvp
{
    [Serializable]
    public class RoundConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private float m_duration = 10.0f;
    }
}