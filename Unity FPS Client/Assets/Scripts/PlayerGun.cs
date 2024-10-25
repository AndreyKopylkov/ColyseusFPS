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

    public bool TryShoot(out ShootInfo info)
    {
        info = new ShootInfo();
        
        if(Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletSpawnPoint.position;
        Vector3 direction = _bulletSpawnPoint.forward;
        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation).
            Initialize(_bulletSpawnPoint.forward, _bulletSpeed);

        direction *= _bulletSpeed;
        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = direction.x;
        info.dY = direction.y;
        info.dZ = direction.z;
        
        OnShoot?.Invoke();

        return true;
    }
}