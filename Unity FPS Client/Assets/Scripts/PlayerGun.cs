using System;
using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _bulletSpeed = 50f;
    [SerializeField] private float _shotFrequency = 200;

    private float _lastShootTime = 0;
    private float _shootDelay;


    private void Start()
    {
        _shootDelay = 60f / _shotFrequency;
    }

    public bool TryShoot(out ShootInfo info)
    {
        info = new ShootInfo();
        
        if(Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletSpawnPoint.position;
        Vector3 velocity = _bulletSpawnPoint.forward * _bulletSpeed;
        
        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation).Initialize(velocity);

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.vX = velocity.x;
        info.vY = velocity.y;
        info.vZ = velocity.z;
        
        OnShoot?.Invoke();

        return true;
    }
}