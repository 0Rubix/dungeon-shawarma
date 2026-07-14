using UnityEngine;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    float currentHealth;

    public float damage;
    public float damageMultiplier;
    public float attackRange;
    public float defense;
    public float attackCooldown;
    public float timeBtwAttacks;
    public float attackAmount;

    [HideInInspector] public float modifiedDamage;
    [HideInInspector] public float modifiedDamageMultiplier;
    [HideInInspector] public float modifiedAttackRange;
    [HideInInspector] public float modifiedDefense;
    [HideInInspector] public float modifiedAttackCooldown;
    [HideInInspector] public float modifiedTimeBtwAttacks;
    [HideInInspector] public float modifiedAttackAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        modifiedDamage = damage;
        modifiedDamageMultiplier = damageMultiplier;
        modifiedAttackRange = attackRange;
        modifiedDefense = defense;
        modifiedAttackCooldown = attackCooldown;
        modifiedTimeBtwAttacks = timeBtwAttacks;
        modifiedAttackAmount = attackAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(float amount)
    {
        float finalDamage = amount >= modifiedDefense ? amount - modifiedDefense : 0f;
        currentHealth -= finalDamage;

        if (currentHealth <= 0f) Die();
    }

    public void Die()
    {
        Debug.Log(name + " died");
    }
}
