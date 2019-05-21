using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    public void PlayerAnimShoot()
    {
        GetComponentInParent<PlayerController>().Fire();
    }

    public void PlayerAnimAttack()
    {
        GetComponentInParent<PlayerController>().AttackCO();
    }

    public void PlayerAnimAttackStop()
    {
        GetComponentInParent<PlayerController>().AttackStop();
    }

    public void PlayerAnimLaser()
    {
        GetComponentInParent<PlayerController>().Laser();
    }

    public void PlayerAnimSniperShoot()
    {
        GetComponentInParent<PlayerController>().SniperShoot();
    }
}
