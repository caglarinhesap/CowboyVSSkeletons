using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private List<Vector3> waypoints = new List<Vector3>();
    private int waypointIndex = 0;
    [SerializeField] float bulletSpeed = 10f;

    void Update()
    {
        FollowPath();
    }

    private void FollowPath()
    {
        if (waypointIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointIndex];
            float delta = bulletSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, delta);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            StartCoroutine(DestroyMissedBulled());
        }
    }

    IEnumerator DestroyMissedBulled()
    {
        yield return new WaitForSeconds(4.0f);
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    public void CreateBulletPath(GameObject projectile, GameObject enemy)
    {
        //Find random waypoint to curvy move

        int hitDirectionHorizontal = Random.Range(0, 2); // 0 = left, 1 = right
        int hitDirectionVertical = Random.Range(0, 2); // 0 = up, 1 = down

        Vector3 middlePoint = (transform.position + enemy.transform.position) / 2;

        if (hitDirectionHorizontal == 0)
        {
            middlePoint.x -= Random.Range(1.0f, 2.4f);
        }
        else
        {
            middlePoint.x += Random.Range(1.0f, 2.4f);
        }

        if (hitDirectionVertical == 0)
        {
            middlePoint.z += Random.Range(1.0f, 2.4f);
        }
        else
        {
            middlePoint.z -= Random.Range(1.0f, 2.4f);
        }

        waypoints.Add(middlePoint);
        waypoints.Add(enemy.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Skeleton")
        {
            GameObject.FindObjectOfType<ScoreSystem>().IncreaseKillCount();
            FindObjectOfType<EnemySpawner>().GetEnemyList().Remove(other.gameObject);
            Animator enemyAnimator = other.GetComponent<Animator>();
            enemyAnimator.SetTrigger("die");
            Destroy(other.gameObject, 3.0f);
            Destroy(gameObject);
        }
    }
}
