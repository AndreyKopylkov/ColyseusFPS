using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[SelectionBase]
public class PlayerCharacter : Character
{
    [Header("PlayerCharacter")]
    [Header("Settings")]
    [SerializeField] private float _minCameraAngle = -90f;
    [SerializeField] private float _maxCameraAngle = 90f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _jumpReload = 0.5f;
    [SerializeField] private float _crawlSizeY = 0.3f;

    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Transform _modelTransform;

    private float _inputH;
    private float _inputV;
    private float _currentCameraRotateX;
    private float _lastJumpTime;

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

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * Speed;
        velocity.y = _rigidbody.linearVelocity.y;
        Velocity = velocity;
        _rigidbody.linearVelocity = Velocity;
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
        if(_checkFly.IsFly)
            return;
        
        if(Time.time - _lastJumpTime < _jumpReload)
            return;

        _lastJumpTime = Time.time;
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = _rigidbody.linearVelocity;

        rotateX = _headTransform.localEulerAngles.x;
        rotateY = _bodyTransform.eulerAngles.y;
    }

    public void GetModelTransformInfo(out Vector3 modelScale)
    {
        modelScale = _modelTransform.localScale;
    }

    public void Crawl()
    {
        if(_checkFly.IsFly)
            return;

        _modelTransform.localScale = new Vector3(1, _crawlSizeY, 1);
    }

    public void StopCrawl()
    {
        _modelTransform.localScale = new Vector3(1, 1, 1);
    }
}
