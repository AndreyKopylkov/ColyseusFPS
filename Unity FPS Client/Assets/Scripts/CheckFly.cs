using System;
using UnityEngine;

public class CheckFly : MonoBehaviour
{
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _coyoteTime = 0.2f;
    
    public bool IsFly { get; private set; }

    private float _flyTimer = 0;

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, _radius, _layerMask))
        {
            IsFly = false;
            _flyTimer = 0;
        }
        else
        {
            _flyTimer += Time.deltaTime;
            
            if(_flyTimer > _coyoteTime)
                IsFly = true;
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
    #endif
}