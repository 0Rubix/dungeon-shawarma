using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    PlayerInput input;
    Entity entity;

    void Awake()
    {
        input = new PlayerInput();
        input.Movement.Enable();
        input.Movement.Attack.performed += Attack;

        entity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        input.Movement.Enable();
    }

    private void OnDisable()
    {
        input.Movement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack(InputAction.CallbackContext ctx)
    {
        if (!entity.canAttack) return;

        Item heldItem = InventoryManager.instance.inventory[InventoryManager.instance.hotbarSlot].item;

        if (heldItem is Weapon w)
        {
            w.Attack(entity);
            if (entity.countForAttack == entity.attackCount.GetModifiedValue()) return;
        }
        else
        {
            StopCoroutine(MainCooldown());

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

            StartCoroutine(MainCooldown());

            if (entity.countForAttack == entity.attackCount.GetModifiedValue())
            {
                entity.canAttack = false;
            }
        }

        if (!entity.canAttack) return;

        StartCoroutine(CooldownBtwAttacks());
    }

    IEnumerator CooldownBtwAttacks()
    {
        entity.canAttack = false;

        yield return new WaitForSeconds(entity.cooldownBtwAttacks.GetModifiedValue());

        entity.canAttack = true;
    }

    IEnumerator MainCooldown()
    {
        yield return new WaitForSeconds(entity.attackCooldown.GetModifiedValue());

        entity.countForAttack = 0;
        entity.anim.SetTrigger("GoBack");
        entity.anim.SetInteger("Counter", 1);
        entity.canAttack = true;
    }
}
