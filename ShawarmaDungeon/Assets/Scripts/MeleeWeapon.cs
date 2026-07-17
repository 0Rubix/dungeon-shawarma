using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Inventory/Melee Weapon")]
public class MeleeWeapon : Weapon
{
    public override void Attack(Entity entity)
    {
        if (entity.mainCooldownRoutine != null)
            entity.StopCoroutine(entity.mainCooldownRoutine);

        entity.countForAttack++;
        entity.anim.SetInteger("Counter", entity.countForAttack);
        entity.anim.SetTrigger("Attack");

        Collider2D[] cols = Physics2D.OverlapCircleAll(entity.attackPoint.position, entity.attackRange.GetModifiedValue(), entity.attackLayer);

        foreach (Collider2D col in cols)
        {
            Entity e = col.GetComponent<Entity>();

            if (e == null) continue;

            e.TakeDamage(entity.attack.GetModifiedValue());
        }

        entity.mainCooldownRoutine = entity.StartCoroutine(MainCooldown(entity));

        if (entity.countForAttack == entity.attackCount.GetModifiedValue())
        {
            entity.canAttack = false;
        }
    }

    public override IEnumerator MainCooldown(Entity e)
    {
        yield return new WaitForSeconds(e.attackCooldown.GetModifiedValue());

        e.countForAttack = 0;
        e.anim.SetTrigger("GoBack");
        e.anim.SetInteger("Counter", 1);
        e.canAttack = true;
    }
}
