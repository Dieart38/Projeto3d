using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class SlimeA : MonoBehaviour
{
    private GameManager _GameManager;
    private Animator anim;
    public ParticleSystem fxBlood;

    public enemyState state;

    public int HP;

    private bool isDie;


    //public const float patrolWaitTime = 7f;

    //IA do Slime
    private NavMeshAgent agent;
    private int idWaypoint;
    private Vector3 destination;

    private bool isWalk;
    private bool isAlert;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        // Use FindAnyObjectByType (mais rápido que o antigo)
        _GameManager = Object.FindAnyObjectByType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        ChangeState(enemyState.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();
        if (agent.desiredVelocity.magnitude >= 0.1f)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }
        anim.SetBool("isWalk", isWalk);
    }

    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);

    }
    #region Meus Metodos

    void GetHit(int amount)
    {
        if (isDie == true) { return; }

        HP -= amount;
        if (HP > 0)
        {
            ChangeState(enemyState.FURY);
            anim.SetTrigger("GetHit");
            fxBlood.Emit(40);
        }
        // 
        if (HP <= 0)
        {
            HP = 0;
            fxBlood.Emit(40);
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }

    void StateManager()
    {
        switch (state)
        {


            case enemyState.ALERT:
                // comportamento em Alert
                break;

            case enemyState.EXPLORE:
                // comportamento em Explore
                break;

            case enemyState.FOLLOW:
                // comportamento em Follow
                break;

            case enemyState.PATROL:
                // comportamento em Idle
                break;

            case enemyState.FURY:
                // comportamento em Fury
                // Atualiza o destino para a posição ATUAL do player a cada frame
                destination = _GameManager.player.position;
                agent.destination = destination;

                // // Opcional: Garante que o Slime não pare longe do player
                // agent.stoppingDistance = 1.2f;
                break;
        }
    }

    void ChangeState(enemyState newState)
    {
        print(newState);
        StopAllCoroutines(); // encerra todas coroutines para não dar erro 
        state = newState;
        switch (state)
        {
            case enemyState.IDLE:
                // comportamento em Idle
                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;

                StartCoroutine("IDLE");
                break;

            case enemyState.ALERT:
                // comportamento em Alert
                break;

            case enemyState.EXPLORE:
                // comportamento em Explore
                break;

            case enemyState.FOLLOW:
                // comportamento em Follow
                break;

            case enemyState.PATROL:
                // comportamento em Patrol
                agent.stoppingDistance = 0;
                idWaypoint = Random.Range(0, _GameManager.slimeWayPoints.Length);
                destination = _GameManager.slimeWayPoints[idWaypoint].position;
                agent.destination = destination;
                StartCoroutine("PATROL");
                break;

            case enemyState.FURY:
                // comportamento em Fury
                destination = transform.position;
                agent.stoppingDistance = _GameManager.slimeDistanceToAttack;
                agent.destination = destination;
                break;
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(_GameManager.slimeIdleWaitTime);
        StayStill(50);

    }

    IEnumerator PATROL()
    {
        yield return new WaitUntil(() => agent.remainingDistance <= 0);
        ChangeState(enemyState.IDLE);
        StayStill(30);
    }

    void StayStill(int yes)
    {
        if (Rand() <= yes)
        {
            ChangeState(enemyState.IDLE);
        }
        else
        {
            ChangeState(enemyState.PATROL);
        }
    }
    int Rand()
    {
        int rand = Random.Range(0, 100);
        return rand;
    }

    #endregion
}
