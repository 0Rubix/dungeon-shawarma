using UnityEngine;

public class RangedWeapon : Weapon
{
    public override void Attack(Entity entity)
    {
        GameObject projectileObject = Instantiate(projectile, entity.attackPoint.position, Quaternion.identity);
    }
}
