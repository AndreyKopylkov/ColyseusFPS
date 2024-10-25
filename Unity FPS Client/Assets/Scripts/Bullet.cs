using UnityEngine;

public  class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public void Initialize(Vector3 direction, float speed)
    {
        _rigidbody.linearVelocity = direction * speed;
    }
}