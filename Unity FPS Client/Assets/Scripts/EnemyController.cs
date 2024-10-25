using System;
using System.Collections.Generic;
using Colyseus.Schema;
using SchemaTest.InstanceSharing;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _enemyCharacter;
    [SerializeField] private EnemyGun _enemyGun;

    Vector3 velocity = Vector3.zero;
    private Queue<float> _receiveTimeIntervalQueue = new Queue<float>();
    private float _lastReceiveTime = 0;
    private Player _player;

    private float AverageInterval
    {
        get
        {
            var e = _receiveTimeIntervalQueue.GetEnumerator();
            float average = 0f;
            while(e.MoveNext())
            {
                average += e.Current;
            }
            average /= _receiveTimeIntervalQueue.Count;
            return average;
        }
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            _receiveTimeIntervalQueue.Enqueue(0);
        }
    }

    public void Initialize(Player player)
    {
        _player = player;
        _enemyCharacter.SetSpeed(_player.speed);
        _player.OnChange += OnChange;
    }

    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private void SaveReceiveTime()
    {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;
        
        _receiveTimeIntervalQueue.Enqueue(interval);
        _receiveTimeIntervalQueue.Dequeue();
    }
    
    public void OnChange(List<DataChange> changes)
    {
        SaveReceiveTime();
        
        Vector3 position = _enemyCharacter.TargetPosition;
        velocity = _enemyCharacter.Velocity;
        
        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "pX":
                    position.x = (float) dataChange.Value;
                    break;
                case "pY":
                    position.y = (float) dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float) dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float) dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float) dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float) dataChange.Value;
                    break;
                case "rX":
                    _enemyCharacter.SetRotateX((float) dataChange.Value);
                    break;
                case "rY":
                    _enemyCharacter.SetRotateY((float) dataChange.Value);
                    break;
                default:
                    Debug.Log($"Не обрабатывается изменение поля {dataChange.Field}");
                    break;
            }
        }

        _enemyCharacter.SetMovement(position, velocity, AverageInterval);
    }

    public void Shoot(in ShootInfo info)
    {
        _enemyGun.Shoot(new Vector3(info.pX, info.pY, info.pZ), new Vector3(info.vX, info.vY, info.vZ));
    }

    public void Crawl(float localScaleY)
    {
        _enemyCharacter.Crawl(localScaleY);
    }
}