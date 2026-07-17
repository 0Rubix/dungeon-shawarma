using System.Collections;
using UnityEngine;

public class Weapon : Item
{
    [Header("Weapon Stats")]
    public float attack;
    public float LastAttackMultiplier;
    public float attackRange;
    public int attackCount;
    public float attackCooldown;
    public float cooldownBtwAttacks;
    public RuntimeAnimatorController controller;

    public virtual void Attack(Entity entity) { }

    public virtual IEnumerator MainCooldown(Entity entity)
    {
        yield return null;
    }
}
