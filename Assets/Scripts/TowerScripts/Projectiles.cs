using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectiles : MonoBehaviour
{
    public float speed = 5f; //adjust projectile speed
    public GameObject impactEffect; //if there is a visual effect on impact assign it here

    private Transform _target;
    public float damage; //damage stat is passed by the tower

    private void Update()
    {
        if (_target == null) 
        {
            //if the target is gone destroy the projectile
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = _target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Check if projectile reaches the target
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Move the projectile
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    public void SetTarget(Transform newTarget) 
    { 
        _target = newTarget;
    }

    private void HitTarget() 
    {
        if (_target != null) 
        {
            EnemyBehaviour enemyHealth = _target.GetComponent<EnemyBehaviour>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        //if there is an impact effect apply it here
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
