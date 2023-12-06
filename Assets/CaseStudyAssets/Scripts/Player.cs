using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private Joystick joystick;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    [SerializeField]
    private float moveSpeed = 23f;
    [SerializeField]
    private float attackRange = 7f;
    [SerializeField]
    private float shootCooldown = 2f;
    [SerializeField]
    private GameObject attackRangeCircle;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject restartPanel;

    private const int SPEED_MULTIPLIER = 375;
//#if UNITY_EDITOR
//    private const int SPEED_MULTIPLIER = 300;
//#endif

//#if UNITY_ANDROID && !UNITY_EDITOR
//    private const int SPEED_MULTIPLIER = 100;
//#endif

    private bool isAlive = true;
    private bool canShoot = true;

    [SerializeField]
    private List<GameObject> enemies;

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        attackRangeCircle.transform.localScale = new Vector3(attackRange, 1, attackRange);
        enemies = FindObjectOfType<EnemySpawner>().GetEnemyList();
    }

    void Update()
    {
        if (isAlive)
        {
            CheckRunning();
            Shoot();
        }
    }

    private void Shoot()
    {
        if (canShoot)
        {
            float distance = float.MaxValue;
            GameObject target = null;

            if (enemies != null && enemies.Count > 0)
            {
                foreach (GameObject gameObject in enemies) // Finds the closest enemy
                {
                    if (gameObject != null)
                    {
                        if (Vector3.Distance(transform.position, gameObject.transform.position) < distance)
                        {
                            distance = Vector3.Distance(transform.position, gameObject.transform.position);
                            target = gameObject;
                        }
                    }
                }

                if (distance <= attackRange && target != null)
                {
                    canShoot = false;
                    GameObject projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    projectile.GetComponent<PlayerBullet>().CreateBulletPath(projectile, target);
                    StartCoroutine(StartShootingCooldown());
                }
            }
        }
    }

    IEnumerator StartShootingCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void CheckRunning()
    {
        if (joystick.Horizontal == 0 && joystick.Vertical == 0) // Not running
        {
            if (playerAnimator.GetBool("isRunning"))
            {
                playerAnimator.SetBool("isRunning", false);
                Stop();
            }
        }
        else
        {
            if (!playerAnimator.GetBool("isRunning"))
            {
                playerAnimator.SetBool("isRunning", true);
            }
            Move();
        }
    }

    private void Move()
    {
        playerRigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed, 0, joystick.Vertical * moveSpeed) * Time.deltaTime * SPEED_MULTIPLIER;

        if (playerRigidbody.velocity != Vector3.zero)
        {
            playerRigidbody.transform.rotation = Quaternion.LookRotation(playerRigidbody.velocity);
        }
    }

    private void Stop()
    {
        playerRigidbody.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "SkeletonProjectile")
        {
            GetComponent<HealthSystem>().TakeDamage();
            Destroy(other.gameObject);
        }
        if (other.tag == "Coin")
        {
            Destroy(other.gameObject);
            GameObject.FindObjectOfType<ScoreSystem>().IncreaseCoinScore();
        }
        if (other.tag == "Skeleton")
        {
            GetComponent<HealthSystem>().FatalDamage();
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        if (isAlive)
        {
            isAlive = false;
            playerAnimator.SetTrigger("die");
            Stop();
            GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>().StopSpawn();
            GameObject.FindGameObjectWithTag("Joystick").SetActive(false);
            restartPanel.SetActive(true);
        }
    }
}
