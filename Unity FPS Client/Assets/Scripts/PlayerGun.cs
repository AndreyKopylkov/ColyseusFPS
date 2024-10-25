using System;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _shotFrequency = 200;

    private float _lastShootTime = 0;
    private float _shootDelay;

    public event Action OnShoot;

    private void Start()
    {
        _shootDelay = 60f / _shotFrequency;
    }

    public void Shoot()
    {
        if(Time.time - _lastShootTime < _shootDelay) return;
        
        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation).
            Initialize(_bulletSpawnPoint.forward, _bulletSpeed);
        
        OnShoot?.Invoke();
    }
}