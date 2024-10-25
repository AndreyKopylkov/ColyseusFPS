using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float _timer = 10f;

    private void Awake()
    {
        Destroy(gameObject, _timer);
    }
}