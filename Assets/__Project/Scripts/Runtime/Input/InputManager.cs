using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PandaIsPandaMvp
{
    public class InputManager : MonoBehaviour, IDisposable
    {
        public event Action<bool, Vector2> OnPointerClick; 
        public event Action<Vector2, Vector2> OnPointerMove; 
        
        private CInputs m_cInputs;

        private bool m_isLeftClick;
        
        private void Start()
        {
            m_cInputs = new CInputs();
            m_cInputs.Board.LeftClick.started   += LeftClick_Click;
            m_cInputs.Board.LeftClick.canceled  += LeftClick_Click;
            
            m_cInputs.Enable();
        }

        private void Update()
        {
            if (m_isLeftClick)
            {
                LeftClick_Move();
            }
        }

        public void Dispose()
        {
            m_cInputs?.Dispose();
        }

        private void LeftClick_Click(InputAction.CallbackContext ctx)
        {
            m_isLeftClick = ctx.ReadValue<float>() > 0.5f;
            
            var position = Pointer.current.position.ReadValue();
            
            OnPointerClick?.Invoke(m_isLeftClick, position);
        }
        
        private void LeftClick_Move()
        {
            var position = Pointer.current.position.ReadValue();

            var delta = Pointer.current.delta.ReadValue();
            
            OnPointerMove?.Invoke(position, delta);
        }
    }
}