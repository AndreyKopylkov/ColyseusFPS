using UnityEngine;

public class DestroySelfTimer : MonoBehaviour
{
    [SerializeField] private float _timer;

    private void Awake()
    {
        Destroy(gameObject, _timer);
    }
}