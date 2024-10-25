using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public void Initialize(Vector3 velocity)
    {
        _rigidbody.linearVelocity = velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}