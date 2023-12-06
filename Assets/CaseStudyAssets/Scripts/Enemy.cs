using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject rightHand;

    private Animator enemyAnimator;

    [SerializeField]
    float attackRange = 8f;
    [SerializeField]
    float moveSpeed = 8f;
    [SerializeField]
    float bulletSpeed = 10f;
    [SerializeField]
    float bulletDestroyTimer = 3f;

    GameObject currentProjectile;
    private bool isShooting;
    private GameObject target;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!isShooting)
        {
            CheckDistance();
        }
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        isShooting = true;
        enemyAnimator.SetBool("isShooting", true);
    }

    private void ChasePlayer()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = targetPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void CreateBullet()
    {
        FocusPlayer();
        currentProjectile = Instantiate(bulletPrefab, rightHand.transform.position, Quaternion.identity, rightHand.transform);
    }

    void ThrowBullet()
    {
        currentProjectile.transform.parent = null;
        Vector3 direction = target.transform.position - rightHand.transform.position;
        direction.y = 0;
        direction.Normalize();  //For getting the unit vector. Different ranges must have same bulletspeed.
        currentProjectile.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

        StartCoroutine(BulletDestroyAnimation(currentProjectile));

        isShooting = false;
        enemyAnimator.SetBool("isShooting", false);
    }

    private void FocusPlayer()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        Vector3 direction = targetPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    IEnumerator BulletDestroyAnimation(GameObject projectile)
    {
        Vector3 substractVector = new Vector3(0.05f, 0.05f, 0.05f);
        float substractTime = 0.05f;
        yield return new WaitForSeconds(bulletDestroyTimer);

        if (projectile != null)
        {
            for (int i = 0; i < 8; i++)
            {
                projectile.transform.localScale -= substractVector;
                projectile.GetComponent<TrailRenderer>().time -= substractTime;

                yield return new WaitForSeconds(0.1f);
            }
            if (projectile != null) //Check if projectile hit to player during destroy animation
            {
                Destroy(projectile);
            }
        }
    }
}
