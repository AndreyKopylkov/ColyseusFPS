using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PlayerGun _playerGun;
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

        playerCharacter.SetInput(_inputH, _inputV);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        playerCharacter.RotateX(-mouseY * _mouseSensitivityVertical);
        playerCharacter.RotateY(mouseX * _mouseSensitivityHorizontal);

        bool space = Input.GetKeyDown(KeyCode.Space);
        
        if(space) playerCharacter.Jump();

        bool isShoot = Input.GetMouseButton(0);
        
        if(isShoot) _playerGun.Shoot();

        SendMove();
    }

    private void SendMove()
    {
        playerCharacter.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX},
            {"rY", rotateY}
        };
        MultiplayerManager.Instance.SendMessage("move", data);
    }
}