using System;
using UnityEngine;

public class EnemyCharacter : Character
{
    [Header("Settings")]
    [SerializeField] private float _rotateSpeed = 10f;
    
    [Header("Components")]
    [SerializeField] private Transform _transformHead;
    [SerializeField] private Transform _transformBody;
    [SerializeField] private Transform _modelTransform;

    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    public Vector3 TargetRotationX { get; private set; } = Vector3.zero;
    public Vector3 TargetRotationY { get; private set; } = Vector3.zero;

    private float _velocityMagnitude = 0;

    private void Start()
    {
        TargetPosition = transform.position;
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if (_velocityMagnitude > 0.1f)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
        }
        else
        {
            transform.position = TargetPosition;
        }
    }

    private void Rotate()
    {
        Quaternion quaternionTarget = Quaternion.Euler(TargetRotationX);
        
        _transformHead.localRotation =
            Quaternion.Slerp(_transformHead.localRotation, quaternionTarget, _rotateSpeed * Time.deltaTime);
        
        if (Quaternion.Angle(_transformHead.localRotation, quaternionTarget) < 2f)
        {
            _transformHead.localRotation = quaternionTarget;
        }

        quaternionTarget = Quaternion.Euler(TargetRotationY);
        _transformBody.localRotation = 
            Quaternion.Slerp(_transformBody.localRotation, quaternionTarget, _rotateSpeed * Time.deltaTime);
        
        if (Quaternion.Angle(_transformBody.localRotation, quaternionTarget) < 2f)
        {
            _transformBody.localRotation = quaternionTarget;
        }
            
        // _transformHead.Rotate(TargetRotationX, _rotateSpeed * Time.deltaTime);
        // _transformBody.Rotate(TargetRotationY, _rotateSpeed * Time.deltaTime);
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        Velocity = velocity;

        TargetPosition = position + Velocity * averageInterval;
        _velocityMagnitude = Velocity.magnitude;
        
    }

    public void SetSpeed(float speed) => Speed = speed;

    public void SetRotateX(float value)
    {
        // _transformHead.localEulerAngles = new Vector3(value, 0, 0);

        TargetRotationX = new Vector3(value, 0, 0);
    }
    
    public void SetRotateY(float value)
    {
        // _transformBody.localEulerAngles = new Vector3(0, value, 0);
        
        TargetRotationY = new Vector3(0, value, 0);
    }
    
    public void Crawl(float localScaleY)
    {
        _modelTransform.localScale = new Vector3(1, localScaleY, 1);
    }
}