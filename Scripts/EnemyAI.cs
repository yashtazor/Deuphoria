using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject fpsc;
    public bool isAware = false;
    public bool isDetecting = false;
    public float fov = 120f;
    public float viewDistance;
    public string wanderType = "Waypoint";
    public Transform[] path;
    public float wanderSpeed = 0.5f;
    public float chaseSpeed = 2f;
    public float loseThreshold = 5f;

    private NavMeshAgent agent;
    private int wayPointIndex = 0;
    private Animator animator;
    private float loseTimer = 0;


    private GameObject player;
    HUD playerHUD;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.Find("The Adventurer Blake (1)");
        playerHUD = player.GetComponent<HUD>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isAware)
        {
            agent.SetDestination(fpsc.transform.position);
            animator.SetBool("Aware", true);
            agent.speed = chaseSpeed;

            //Attack the player
            float dist = Vector3.Distance(fpsc.transform.position, transform.position);
            if (dist < 6f)
            {
                animator.SetTrigger("Attack");
                Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
                foreach (Collider player in hitPlayer)
                {
                    Debug.Log("We hit player");
                    playerHUD.sendDamage(5);
                }
            }



            if (!isDetecting)
            {
                loseTimer += Time.deltaTime;

                if(loseTimer >= loseThreshold)
                {
                    isAware = false;
                    loseTimer = 0;
                }
            }
        }
        else
        {
            wander();
            animator.SetBool("Aware", false);
            agent.speed = wanderSpeed;
        }

        SearchPlayer();
    }


    public void SearchPlayer()
    {
        if(Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(fpsc.transform.position)) < (fov / 2f))
        {
            if (Vector3.Distance(fpsc.transform.position, transform.position) < viewDistance)
            {
                RaycastHit hit;

                if (Physics.Linecast(transform.position, fpsc.transform.position, out hit, -1))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                    else
                    {
                        isDetecting = false;
                    }
                }
                else
                {
                    isDetecting = false;
                }
            }
            else
            {
                isDetecting = false;
            }
        }
        else
        {
            isDetecting = false;
        }
    }


    public void OnAware()
    {
        isAware = true;
        isDetecting = true;
        loseTimer = 0;
    }


    public void wander()
    {
        if(Vector3.Distance(path[wayPointIndex].position, transform.position) < 2f)
        {
            wayPointIndex = (wayPointIndex + 1) % 4;
        }
        else
        {
            agent.SetDestination(path[wayPointIndex].position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}
