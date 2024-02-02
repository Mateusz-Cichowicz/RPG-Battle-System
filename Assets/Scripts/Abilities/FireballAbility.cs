using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class FireballAbility : Ability
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] int damage;
    [SerializeField] float radius;
    public override void Activate(Character character)
    {
        if (fireballPrefab != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, character.transform);
            Move fireballMove = fireball.GetComponent<Move>();

            if (fireballMove != null)
            {
                fireballMove.target = character.target;
                fireballMove.ObjectDestroyed.AddListener(ApplyExplosionDamage);
            }
        }
        else
        {
            Debug.LogError("FireballPrefab not assigned in the Unity Editor.");
        }
    }
    void ApplyExplosionDamage(Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Character character))
            {
                hitCollider.SendMessage("TakeDamage", damage);
            }
        }
    }
    
}
//Should it be here?
[System.Serializable]
public class MyVector3Event : UnityEvent<Vector3> { }
