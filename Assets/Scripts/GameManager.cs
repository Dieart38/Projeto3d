using UnityEngine;

public enum enemyState
{
    IDLE, ALERT, EXPLORE, PATROL, FOLLOW, FURY
}
public class GameManager : MonoBehaviour
{
    public Transform player;
    [Header("Slime IA")]
    public float slimeIdleWaitTime = 4f;
    public Transform[] slimeWayPoints;
    public float slimeDistanceToAttack = 10f;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
