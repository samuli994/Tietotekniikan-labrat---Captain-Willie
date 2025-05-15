using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WillieAi : MonoBehaviour
{
    //public Fade fade;
    public List<Transform> transforms;
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

    //public Animator anim;

    //Sounds
    public AudioSource audioSource;
    //public AudioSource braam;
    //States
    public float sightRange, attackRange, dyingRange;
    public static bool playerInSightRange, playerInAttackRange, playerDyingRange;
    public static bool playerEntersMonsterArea = false;

    private Animator anim;
    private bool waitingAtWalkpoint = false;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public AudioSource blowoutCandle;

    public List<AudioClip> clips;
    private AudioClip lastPlayedClip;
    private bool hunting;
    public bool roamer = false;
    public FirstPersonController fpc;
    private bool firsttime = true;
    public GameObject nightVisionLight;
    public MeshRenderer playerCandle;
    public GameObject playerCandleLight;
    public GameObject monsterWillie;
    private bool routineStarted;
    public AudioSource walkingAudio;
    public SpeechManager speechManager;
    public bool onTrigger = false;


    // Start is called before the first frame update

    private void Start() {
        anim = GetComponent<Animator>();
        fpc = FindAnyObjectByType<FirstPersonController>();
        //audioSource = GetComponent<AudioSource>();
        //GameObject playerobject = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    public AudioClip chooseClip()
    {
        if (clips.Count == 0)
        {
            Debug.LogWarning("No audio clips available.");
            return null;
        }
        // Get a random index different from the last played clip
        int randomIndex = Random.Range(0, clips.Count);
        while (clips[randomIndex] == lastPlayedClip)
        {
            randomIndex = Random.Range(0, clips.Count);
        }
        lastPlayedClip = clips[randomIndex];
        return lastPlayedClip;
    }

    private void Update() {

        if (!routineStarted && onTrigger) {
            StopPlayerActions();
        }

        if (!roamer) {
            return;
        }

        if (chaseTimer > 0)
        {
            chaseTimer -= Time.deltaTime;
        }

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position,sightRange, whatIsPlayer);

        if (CanSeePlayer(playerobject)) 
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

        if (!playerInAttackRange && !playerInSightRange) 
        {
            Patroling();
        }

        if (chaseTimer <= 0 && hunting == true) {
            
            if(CanSeePlayer(playerobject)) {
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
            RaycastHit hit;
            if (Physics.Raycast(eyesTransform.position, directionToPlayer, out hit, sightLightRange, raycastMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
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
        if (routineStarted) {
            return;
        }
        if (firsttime) 
        {
            anim.SetBool("walking",true);
            fpc.playerCanMove = false;
            fpc.cameraCanMove = false;
            fpc.enableHeadBob = false;
            fpc.walkingAudio.mute = true;
            firsttime = false;
        }
        walkingAudio.Play();
        fpc.transform.LookAt(eyesTransform.transform);
        WalkToPlayer();
        // Make the agent look towards the player
        transform.LookAt(player);
    }

    private void WalkToPlayer() {
        Transform closestTransform = Vector3.Distance(player.position, transforms[0].position) < Vector3.Distance(player.position, transforms[1].position) ? transforms[0] : transforms[1];
        agent.SetDestination(new Vector3(closestTransform.position.x,gameObject.transform.position.y,closestTransform.position.z));
        Vector3 vector1 = new Vector3(closestTransform.position.x,gameObject.transform.position.y,closestTransform.position.z);
        Vector3 vector2 = transform.position;
        float distance = Vector3.Distance(vector1,vector2);
        speechManager.ShowText(8,"Puhu1");
        if (distance < 0.8f) {
            walkingAudio.Stop();
            agent.SetDestination(gameObject.transform.position);
            anim.SetBool("walking",false);
            trigger.SetActive(false);
            StartCoroutine(candleBlow());
        }
    }

    private IEnumerator candleBlow() 
    {
        routineStarted = true;
        audioSource.clip = chooseClip();
        audioSource.Play();
        yield return new WaitForSeconds(5f);
        speechManager.ShowText(8,"Puhu2");
        audioSource.clip = chooseClip();
        audioSource.Play();
        blowoutCandle.Play();
        yield return new WaitForSeconds(0.8f);
        agent.SetDestination(transforms[2].position);
        walkingAudio.Play();
        anim.SetBool("walking",true);
        playerCandle.enabled = false;
        playerCandleLight.SetActive(false);
        nightVisionLight.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        agent.SetDestination(gameObject.transform.position);
        anim.SetBool("walking",false);
        yield return new WaitForSeconds(1f);
        monsterWillie.SetActive(true);
        fpc.playerCanMove = true;
        fpc.cameraCanMove = true;
        fpc.enableHeadBob = true;
        fpc.walkingAudio.mute = false;
        gameObject.SetActive(false);
    }


    private void ChasePlayer()
    {

    }
    private void Patroling() {
        //anim.speed = 0.5f;
        if (!walkPointSet) {
            SearchWalkPoint();
        }
        if (walkPointSet) {
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
        if (distanceToWalkPoint.magnitude < 4f) {
            walkPointSet = false;
        } 
    }


    private void SearchWalkPoint() {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);
        float randomy = Random.Range(-5, 5);

        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y + 0, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkPointSet = true;
        }
    }


    private void AttackPlayer()
    {
       
    }

    private void OnDrawGizmosSelected() {
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