using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    
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

        playerMovement.SetInput(_inputH, _inputV);
        
        SendMove();
    }

    private void SendMove()
    {
        playerMovement.GetMoveInfo(out Vector3 position, out Vector3 velocity);
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