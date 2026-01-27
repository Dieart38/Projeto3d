using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class SlimeA : MonoBehaviour
{
    private Animator anim;
    public ParticleSystem fxBlood;

    public enemyState state;

    public int HP;

    private bool isDie;

    public const float idleWaitTime = 3f;
    public const float patrolWaitTime = 5f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        anim = GetComponent<Animator>(); // 
        ChangeState (enemyState.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        //
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
            case enemyState.IDLE:
                // comportamento em Idle
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
                // comportamento em Idle
                break;

            case enemyState.FURY:
                // comportamento em Futy
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
                // comportamento em Idle
                StartCoroutine("PATROL");
                break;

            case enemyState.FURY:
                // comportamento em Futy
                break;
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(idleWaitTime);
        if(Rand() <= 50)
        {
            ChangeState(enemyState.IDLE);
        }
        else
        {
            ChangeState(enemyState.PATROL);
        }
        
    }

    IEnumerator PATROL()
    {
        yield return new WaitForSeconds(patrolWaitTime);
        ChangeState(enemyState.IDLE);
    }

    int Rand()
    {
        int rand = Random.Range(0,100);
        return rand; 
    }

    #endregion
}
