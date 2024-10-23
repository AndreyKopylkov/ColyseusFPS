using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _minCameraAngle = -90f;
    [SerializeField] private float _maxCameraAngle = 90f;
    [SerializeField] private float _jumpForce = 10f;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Transform _cameraPoint;

    private float _inputH;
    private float _inputV;
    private float _currentCameraRotateX;

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.SetParent(_cameraPoint);
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

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
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }

    public void RotateX(float value)
    {
        _currentCameraRotateX = Mathf.Clamp(_currentCameraRotateX + value, _minCameraAngle, _maxCameraAngle);
        _headTransform.localEulerAngles = new Vector3(_currentCameraRotateX, 0, 0);

        // _headTransform.Rotate(value, 0, 0);
    }
    
    public void RotateY(float value)
    {
        _bodyTransform.Rotate(0, value, 0);
    }

    public void Jump()
    {
        if(CheckGround())
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }

    private bool CheckGround()
    {
        return true;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
    {
        position = transform.position;
        velocity = _rigidbody.linearVelocity;
    }
}
