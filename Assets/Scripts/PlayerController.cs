using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator anim;
    [Header("Config Player")]
    public float movementSpeed = 5.0f;
    private Vector3 direction;
    private bool isWalk = false;

    private float horizontal;
    private float vertical;
    [Header("Attack Config")]
    public ParticleSystem fxAttack;
    private bool isAttacking = false;
    public Transform hitBox;
    [Range(0.2f, 1f)]
    public float hitRange = 0.5f;
    public LayerMask hitMask;
    public Collider[] hitInfo;
    public int amountDamage;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        MoveCharacter();
        UpdateAnimations();

    }

    #region Meus Metodos

    void Inputs()
    {
        // Get input from keyboard
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //Attack Input
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        fxAttack.Play();

       hitInfo = Physics.OverlapSphere(hitBox.position, hitRange, hitMask);

        foreach (Collider c in hitInfo)
        {
            // Send message to the hit object to call GetHit method
            c.gameObject.SendMessage("GetHit", amountDamage, SendMessageOptions.DontRequireReceiver);
            
        }

    }
    void MoveCharacter()
    {
        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calculate the target angle
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // Rotate the player to face the movement direction
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            // Play walking animation if not already playing

            isWalk = true;

        }
        else
        {
            // Stop walking animation if no input
            isWalk = false;

        }
        // Move the player
        characterController.Move(direction * movementSpeed * Time.deltaTime);


    }

    void UpdateAnimations()
    {
        anim.SetBool("isWalk", isWalk);
    }

    void AttackIsDone()
    {
        isAttacking = false;
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        if (hitBox != null) // é necessário para evitar erros caso o HitBox não esteja atribuído
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBox.position, hitRange);
        }
    }
}
