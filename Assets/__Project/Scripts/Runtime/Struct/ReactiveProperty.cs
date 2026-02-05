using System;
using UnityEngine;

[Serializable]
public class ReactiveProperty<T>
{
    [SerializeField] private T m_value;

    public delegate void ClickedHandler(T value);
    
    public delegate void ChangedHandler(T oldValue, T newValue);
    
    public event ClickedHandler OnValueChanged; 
    
    public event ChangedHandler OnValueChangedTween; 
        
    public T Value
    {
        get => m_value;
        set
        {
            if (Equals(m_value, value)) 
                return;

            var prev = m_value;
            
            m_value = value;
            
            OnValueChanged?.Invoke(m_value);
            OnValueChangedTween?.Invoke(prev, m_value);
        }
    }

    public ReactiveProperty(T initialValue)
    {
        m_value = initialValue;
    }
}