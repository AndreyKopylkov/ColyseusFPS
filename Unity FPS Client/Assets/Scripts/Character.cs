using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character")]
    [field: SerializeField] public float Speed { get; protected set; } = 3f;
    public Vector3 Velocity { get; protected set; }
}