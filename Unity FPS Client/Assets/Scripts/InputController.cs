using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float _mouseSensitivityVertical = 2f;
    [SerializeField] private float _mouseSensitivityHorizontal = 2f;
    
    private float _inputH;
    private float _inputV;
    
    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        _inputH = Input.GetAxisRaw("Horizontal");
        _inputV = Input.GetAxisRaw("Vertical");

        _playerMovement.SetInput(_inputH, _inputV);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        _playerMovement.RotateX(-mouseY * _mouseSensitivityVertical);
        _playerMovement.RotateY(mouseX * _mouseSensitivityHorizontal);

        bool space = Input.GetKeyDown(KeyCode.Space);
        
        if(space)
            _playerMovement.Jump();

        SendMove();
    }

    private void SendMove()
    {
        _playerMovement.GetMoveInfo(out Vector3 position, out Vector3 velocity);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z}
        };
        MultiplayerManager.Instance.SendMessage("move", data);
    }
}