using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;

    [SerializeField] float rotationSpeed = 1f;
    Transform target;

    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }
        target = closestTarget;
    }

    void AimWeapon()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        Attack(targetDistance < range);

        // Calculate the rotation to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

        // Use Slerp for smooth interpolation
        weapon.rotation = Quaternion.Slerp(weapon.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        //weapon.LookAt(target);
    }
    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Cor da esfera
        Gizmos.DrawWireSphere(transform.position, range); // Desenha uma esfera com o centro na posição da torreta e o raio igual ao alcance
    }
}
