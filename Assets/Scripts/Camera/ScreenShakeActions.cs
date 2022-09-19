using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] float grenadeShakeForce = 2f;

    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void ShootAction_OnAnyShoot(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake();
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(grenadeShakeForce);
    }    

    private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
