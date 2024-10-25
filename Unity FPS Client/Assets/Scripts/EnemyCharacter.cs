using System;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform _transformHead;
    [SerializeField] private Transform _transformBody;
    
    public Vector3 TargetPosition { get; private set; } = Vector3.zero;

    private float _velocityMagnitude = 0;

    private void Start()
    {
        TargetPosition = transform.position;
    }

    private void Update()
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

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        Velocity = velocity;

        TargetPosition = position + Velocity * averageInterval;
        _velocityMagnitude = Velocity.magnitude;
        
    }

    public void SetSpeed(float speed) => Speed = speed;

    public void SetRotateX(float value)
    {
        _transformHead.localEulerAngles = new Vector3(value, 0, 0);
    }
    
    public void SetRotateY(float value)
    {
        _transformBody.localEulerAngles = new Vector3(0, value, 0);
    }
}