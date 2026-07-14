using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item
{
    [Header("Weapon")]
    public AnimatorController controller;
    public float attack = 1;
    public float attackMultiplier = 1;
    public float attackRange = 1;
    public int attackAmount = 1;
    public float parrySpeed = 1;
    public float attackCooldown = 1;
    public float cooldownBtwAttacks = 0.1f;
}
