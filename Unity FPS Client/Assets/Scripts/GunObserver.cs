using UnityEngine;

public class GunObserver : MonoBehaviour
{
    [SerializeField] private PlayerGun _playerGun;
    [SerializeField] private Animator _animator;
    
    private const string _shoot = "Shoot";

    private void OnEnable()
    {
        _playerGun.OnShoot += OnShoot;
    }

    private void OnDisable()
    {
        _playerGun.OnShoot -= OnShoot;
    }

    private void OnShoot()
    {
        _animator.SetTrigger(_shoot);
    }
}