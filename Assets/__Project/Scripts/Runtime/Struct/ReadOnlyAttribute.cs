using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute
{
    public readonly bool m_isRuntimeOnly;

    public ReadOnlyAttribute(bool isRuntimeOnly = false)
    {
        m_isRuntimeOnly = isRuntimeOnly;
    }
}