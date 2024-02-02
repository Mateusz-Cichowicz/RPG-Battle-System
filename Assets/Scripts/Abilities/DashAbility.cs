using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public override void Activate() 
    {
        Debug.Log("i'm dashing");
    }
}
