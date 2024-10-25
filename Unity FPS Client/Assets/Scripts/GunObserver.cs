using UnityEngine;

public class GunObserver : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;
    
    private const string _shoot = "Shoot";

    private void OnEnable()
    {
        _gun.OnShoot += OnShoot;
    }

    private void OnDisable()
    {
        _gun.OnShoot -= OnShoot;
    }

    private void OnShoot()
    {
        _animator.SetTrigger(_shoot);
    }
}