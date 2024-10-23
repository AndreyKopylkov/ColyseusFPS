using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Rigidbody _rigidbody;

    private float _inputH;
    private float _inputV;
    
    private void FixedUpdate()
    {
        Move();
    }

    public void SetInput(float inputH, float inputV)
    {
        _inputH = inputH;
        _inputV = inputV;
    }

    private void Move()
    {
        // Vector3 direction = new Vector3(_inputH, 0, _inputV).normalized;
        // transform.position += direction * Time.deltaTime * _speed;

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        _rigidbody.linearVelocity = velocity;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
    {
        position = transform.position;
        velocity = _rigidbody.linearVelocity;
    }
}
