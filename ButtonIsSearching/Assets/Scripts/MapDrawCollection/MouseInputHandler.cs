using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MapDrawCollection
{
    public class MouseInputHandler
    {
        private static MouseInputHandler _mouseInputHandler;

        public readonly Camera _camera;
        public readonly PlayerInput _playerInput;
        public Vector3 WorldPosMouse { get; set; }
        public bool HoldActive { get; set; }

        private MouseInputHandler()
        {
            _playerInput = new PlayerInput();
            _camera = Camera.main;
        }

        public static MouseInputHandler GetInstance()
        {
            if (_mouseInputHandler == null)
            {
                _mouseInputHandler = new MouseInputHandler();
            }
            return _mouseInputHandler;
        }

        public void OnEnable()
        {
            _playerInput.Enable();
     
            _playerInput.Gameplay.MousePosition.performed += OnMouseMove;
     
            _playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
            _playerInput.Gameplay.MouseLeftClick.started += OnLeftClick;
            _playerInput.Gameplay.MouseLeftClick.canceled += OnLeftClick;
     
            _playerInput.Gameplay.MouseRightClick.performed += OnRightClick;
        }
         
        private void OnRightClick(InputAction.CallbackContext obj)
        {
            BuildingCreator.GetInstance().BuildingButtonHandler.UnsetDrawMode();
        }
     
        private void OnLeftClick(InputAction.CallbackContext ctx)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (ctx.phase == InputActionPhase.Started)
                {
                    HoldActive = true;
                }
            }
            if (ctx.phase == InputActionPhase.Canceled || ctx.phase == InputActionPhase.Performed)
            {
                HoldActive = false;
            }
        }
     
        private void OnMouseMove(InputAction.CallbackContext ctx)
        {
            Vector2 mousePos = ctx.ReadValue<Vector2>();
            WorldPosMouse = _camera.ScreenToWorldPoint(mousePos);
        }
         
        public void OnDisable()
        {
            _playerInput.Disable();
     
            _playerInput.Gameplay.MousePosition.performed -= OnMouseMove;
     
            _playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
            _playerInput.Gameplay.MouseLeftClick.started -= OnLeftClick;
            _playerInput.Gameplay.MouseLeftClick.canceled -= OnLeftClick;
     
            _playerInput.Gameplay.MouseRightClick.performed -= OnRightClick;
        }
        
    }
}