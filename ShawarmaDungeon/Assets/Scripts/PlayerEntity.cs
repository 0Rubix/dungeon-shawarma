using UnityEngine;

public class PlayerEntity : Entity
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        PlayerManager.instance.onHeldItemChanged += ModifyStats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ModifyStats(Item item)
    {
        if (item is Weapon w)
        {
            modifiedDamage = w.attack;
            modifiedDamageMultiplier = w.attackMultiplier;
            modifiedAttackRange = w.attackRange;
            modifiedAttackCooldown = w.attackCooldown;
            modifiedTimeBtwAttacks = w.cooldownBtwAttacks;
            modifiedAttackAmount = w.attackAmount;
        }
        else
        {
            modifiedDamage = damage;
            modifiedDamageMultiplier = damageMultiplier;
            modifiedAttackRange = attackRange;
            modifiedAttackCooldown = attackCooldown;
            modifiedTimeBtwAttacks = timeBtwAttacks;
            modifiedAttackAmount = attackAmount;
        }
    }
}
