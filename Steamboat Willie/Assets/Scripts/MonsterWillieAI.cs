using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterWillieAI : MonoBehaviour
{
    //public Fade fade;
    public GameObject trigger;
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public LayerMask raycastMask;
    public float chaseSpeed;
    public float normalSpeed;
    public float chaseTimer;
    public GameObject playerobject;
    private bool chasing = false;
    public float fieldOfViewAngle = 110f;
    public float sightLightRange = 10f;
    public Transform eyesTransform;
    //private Vector3 savePosition;
    //Sounds
    public AudioSource audioSource;
    //public AudioSource braam;
    //States
    public float sightRange, attackRange, dyingRange;
    public static bool playerInSightRange, playerInAttackRange, playerDyingRange;
    public static bool playerEntersMonsterArea = false;

    private Animator anim;
    private bool waitingAtWalkpoint = false;

    public AudioSource walkingAudio;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public bool hunting;
    public bool roamer = false;

    public FirstPersonController fpc;
    private bool firsttime = true;

    public MeshRenderer playerCandle;
    public GameObject playerCandleLight;
    public GameObject nightVisionLight;
    public bool chase;
    public AudioSource scream;
    private bool killing = false;

    // Start is called before the first frame update

    private void Start()
    {
        anim = GetComponent<Animator>();
        fpc = FindAnyObjectByType<FirstPersonController>();
        //audioSource = GetComponent<AudioSource>();
        //GameObject playerobject = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        if (hunting)
        {
            agent.speed = 4;
            ChasePlayer();
        }
        else
        {
            agent.speed = 1;
        }

        if (chase)
        {
            hunting = true;
            ChasePlayer();
        }

        if (chaseTimer > 0)
        {
            chaseTimer -= Time.deltaTime;
        }

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (CanSeePlayer(playerobject) || chase)
        {
            hunting = true;
            if (chaseTimer == 0)
            {
                chaseTimer = 10f;
            }
        }

        if (playerInAttackRange)
        {
            AttackPlayer();
        }

        if (!playerInAttackRange && !playerInSightRange && hunting == false)
        {
            walkingAudio.pitch = 0.56f;
            Patroling();
        }

        if (chaseTimer <= 0 && hunting == true)
        {

            if (CanSeePlayer(playerobject))
            {
                chaseTimer += 1f;
            }
            else
            {
                //audioSource.Stop();
                chaseTimer = 0f;
                hunting = false;
                Patroling();
            }
        }

    }

    private bool CanSeePlayer(GameObject player)
    {
        Vector3 directionToPlayer = player.transform.position - eyesTransform.position;
        float angleToPlayer = Vector3.Angle(directionToPlayer, eyesTransform.forward);

        if (angleToPlayer <= fieldOfViewAngle * 0.5f)
        {
            Debug.DrawRay(eyesTransform.position, directionToPlayer * 15, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(eyesTransform.position, directionToPlayer, out hit, sightLightRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Näki pelaajan");
                    return true;
                }
            }
        }

        return false;
    }

    public void StopPlayerActions()
    {
        // Stop the player's actions (you might want to add more actions to stop)
        // For example, if the player has a script controlling movement, you can disable it

        if (firsttime)
        {
            anim.SetBool("walking", true);
            fpc.playerCanMove = false;
            fpc.cameraCanMove = false;
            fpc.enableHeadBob = false;
            firsttime = false;
        }

        fpc.transform.LookAt(eyesTransform.transform);
        // Set the agent's destination to a point 0.5 units away from the player
        Vector3 destinationPoint = player.position;
        agent.SetDestination(destinationPoint);

        Vector3 distanceToWalkPoint = transform.position - new Vector3(destinationPoint.x, transform.position.y, destinationPoint.z);
        Debug.Log(distanceToWalkPoint.magnitude);
        if (distanceToWalkPoint.magnitude < 1.5f)
        {
            agent.SetDestination(gameObject.transform.position);
            Debug.Log("Stop walking");
            anim.SetBool("walking", false);
            trigger.SetActive(false);
            StartCoroutine(candleBlow());
        }
        // Make the agent look towards the player
        transform.LookAt(player);
    }

    private IEnumerator candleBlow()
    {
        // TODO PUHU JOTAIN
        yield return new WaitForSeconds(5f);
        agent.SetDestination(new Vector3(3.91000009f, -15.70648f, -3.72000003f));
        yield return new WaitForSeconds(5f);
        //Sammuta valo ja spawnaa mörkö
        playerCandle.enabled = false;
        playerCandleLight.SetActive(false);
        nightVisionLight.SetActive(true);
        fpc.playerCanMove = true;
        fpc.cameraCanMove = true;
        fpc.enableHeadBob = true;
    }


    private void ChasePlayer()
    {
        walkingAudio.pitch = 1f;
        anim.SetFloat("Speed", 0.6f);
        agent.SetDestination(player.transform.position);
        agent.transform.LookAt(player.transform);
    }
    private void Patroling()
    {
        walkingAudio.pitch = 0.56f;
        anim.SetFloat("Speed", 0.4f);
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            // Check if the agent is almost standing still
            if (agent.velocity.magnitude < 0.1f)
            {
                Debug.Log("Jumi estetty");
                // Generate a new walkpoint
                walkPointSet = false;
            }
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 4f)
        {
            walkPointSet = false;
        }
    }


    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);
        float randomy = Random.Range(-5, 5);

        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y + 0, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }


    private void AttackPlayer()
    {
        if (!anim.GetBool("Kill"))
        {
            anim.SetBool("Kill", true);
        }
        transform.LookAt(player);
        agent.SetDestination(transform.position);
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        fpc.isZoomed = true;
        fpc.playerCanMove = false;
        fpc.cameraCanMove = false;
        player.transform.LookAt(new Vector3(eyesTransform.position.x, eyesTransform.position.y - 0.9f, eyesTransform.position.z));
        if (!killing)
        {
            StartCoroutine(KillWithDelay());
        }

    }

    public IEnumerator KillWithDelay()
    {
        killing = true;
        scream.Play();
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.day8reset = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Vector3 frontDir = eyesTransform.forward;
        Vector3 leftDir = Quaternion.Euler(0f, -fieldOfViewAngle * 0.5f, 0f) * frontDir;
        Vector3 rightDir = Quaternion.Euler(0f, fieldOfViewAngle * 0.5f, 0f) * frontDir;

        Gizmos.color = new Color(1f, 1f, 0f, 0.1f); // Yellow with transparency
        Gizmos.DrawLine(eyesTransform.position, eyesTransform.position + leftDir * sightLightRange);
        Gizmos.DrawLine(eyesTransform.position, eyesTransform.position + rightDir * sightLightRange);
        Gizmos.DrawLine(eyesTransform.position, eyesTransform.position + frontDir * sightLightRange);
        Gizmos.DrawRay(eyesTransform.position, leftDir * sightLightRange);
        Gizmos.DrawRay(eyesTransform.position, rightDir * sightLightRange);
    }

}