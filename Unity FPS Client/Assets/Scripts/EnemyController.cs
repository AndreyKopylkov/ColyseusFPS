using System;
using System.Collections.Generic;
using Colyseus.Schema;
using SchemaTest.InstanceSharing;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyMovement _enemyMovement;

    Vector3 velocity = Vector3.zero;
    private Queue<float> _receiveTimeIntervalQueue = new Queue<float>();
    private float _lastReceiveTime = 0;

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
        
        Vector3 position = _enemyMovement.TargetPosition;
        
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
                default:
                    Debug.Log($"Не обрабатывается изменение поля {dataChange.Field}");
                    break;
            }
        }

        _enemyMovement.SetMovement(position, velocity, AverageInterval);
    }
}