using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] PlayerEntity entity;
    PlayerInput input;

    [SerializeField] private bool canAttack = true;
    bool attacking = false;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask attackable;
    [SerializeField] private Animator anim;

    float attackCooldown;
    int attackCounter = 0;

    void Awake()
    {
        entity = GetComponent<PlayerEntity>();

        input = new PlayerInput();
        input.Movement.Enable();
        input.Movement.Attack.performed += Attack;
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
        if (attacking)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                anim.SetTrigger("GoBack");
                attackCounter = 0;
                anim.SetInteger("Counter", attackCounter);
                attacking = false;
                canAttack = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint == null ? transform.position : attackPoint.position, entity.modifiedAttackRange);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!canAttack) return;

        attacking = true;

        attackCounter++;
        anim.SetTrigger("Attack");
        anim.SetInteger("Counter", attackCounter);

        attackCooldown = entity.modifiedAttackCooldown;

        Collider2D[] cols = Physics2D.OverlapCircleAll(attackPoint.position, entity.modifiedAttackRange, attackable);

        foreach (var col  in cols)
        {
            Entity colEntity = col.GetComponent<Entity>();
            if (colEntity == null) return;
            colEntity.TakeDamage(entity.modifiedDamage * (attackCounter == entity.modifiedAttackAmount ? entity.modifiedDamageMultiplier : 1));
        }

        if (attackCounter == entity.modifiedAttackAmount)
        {
            canAttack = false;
            return;
        }

        StartCoroutine(TimeBetweenAttacksCooldown());
    }

    IEnumerator TimeBetweenAttacksCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(entity.modifiedTimeBtwAttacks);

        canAttack = true;
    }
}
