using System.Collections.Generic;
using Colyseus.Schema;
using SchemaTest.InstanceSharing;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public void OnChange(List<DataChange> changes)
    {
        Vector3 position = transform.position;
        
        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "x":
                    position.x = (float) dataChange.Value;
                    break;
                case "z":
                    position.z = (float) dataChange.Value;
                    break;
                default:
                    Debug.Log($"Не обрабатывается изменение поля {dataChange.Field}");
                    break;
            }
        }

        transform.position = position;
    }
}