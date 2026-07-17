using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Stat maxHealth;
    float currentHealth;

    public Stat attack;
    public Stat attackMultiplier;
    public Stat attackRange;
    public Transform attackPoint;

    public Stat attackCount;
    public Stat attackCooldown;
    public Stat cooldownBtwAttacks;
    public LayerMask attackLayer;

    public Coroutine mainCooldownRoutine;

    public Animator anim;
    public bool canAttack = true;

    public int countForAttack;

    private void Awake()
    {
        currentHealth = maxHealth.GetModifiedValue();
        countForAttack = 0;
    }

    private void Start()
    {
        PlayerManager.instance.onHeldItemChanged += UpdateStats;
    }

    public void TakeDamage(float amount)
    {
        float finalDamage = amount;
        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            Die();
        } 
    }

    void Die()
    {
        Debug.Log(name + " Died!");
    }

    void UpdateStats(Item item)
    {
        if (item is Weapon w)
        {
            attack.ModifyValue(w.attack);
            attackMultiplier.ModifyValue(w.LastAttackMultiplier);
            attackRange.ModifyValue(w.attackRange);
            attackCount.ModifyValue(w.attackCount);
            attackCooldown.ModifyValue(w.attackCooldown);
            cooldownBtwAttacks.ModifyValue(w.cooldownBtwAttacks);
        }
        else
        {
            attack.ResetValue();
            attackMultiplier.ResetValue();
            attackRange.ResetValue();
            attackCount.ResetValue();
            attackCooldown.ResetValue();
            cooldownBtwAttacks.ResetValue();
        }

        countForAttack = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange.GetModifiedValue());
    }
}
