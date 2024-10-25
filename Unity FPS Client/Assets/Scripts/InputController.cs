using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PlayerGun _playerGun;
    [SerializeField] private float _mouseSensitivityVertical = 2f;
    [SerializeField] private float _mouseSensitivityHorizontal = 2f;

    private MultiplayerManager _multiplayerManager;
    private float _inputH;
    private float _inputV;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }

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

        if (isShoot && _playerGun.TryShoot(out ShootInfo info)) SendShoot(ref info);

        bool isCrawl = Input.GetKeyDown(KeyCode.LeftControl);

        if (isCrawl) playerCharacter.Crawl();

        SendMove();
    }

    private void SendShoot(ref ShootInfo info)
    {
        info.key = _multiplayerManager.GetSessionID();
        string json = JsonUtility.ToJson(info);
        _multiplayerManager.SendMessage("shoot", json);
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
        _multiplayerManager.SendMessage("move", data);
    }
}

[System.Serializable]
public struct ShootInfo
{
    public string key;
    
    //Position
    public float pX;
    public float pY;
    public float pZ;

    //Velocity
    public float vX;
    public float vY;
    public float vZ;
}