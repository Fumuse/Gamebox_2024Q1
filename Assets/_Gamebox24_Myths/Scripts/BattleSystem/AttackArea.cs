using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public List<IDamagable> DamageablesInRange { get; } = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damageable))
        {
            DamageablesInRange.Add(damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damageable) && DamageablesInRange.Contains(damageable))
        {
            DamageablesInRange.Remove(damageable);
        }
    }
}