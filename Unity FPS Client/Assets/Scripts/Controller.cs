using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    
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

        _movement.SetInput(_inputH, _inputV);
    }
}
