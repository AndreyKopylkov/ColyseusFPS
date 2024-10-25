using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Bullet _bulletPrefab;

    public Action OnShoot;
}