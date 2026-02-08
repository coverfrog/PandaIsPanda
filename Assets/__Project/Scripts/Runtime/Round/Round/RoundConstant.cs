using System;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class RoundConstant
    {
        [SerializeField] private ulong m_id;
        [SerializeField] private float m_duration = 10.0f;
    }
}