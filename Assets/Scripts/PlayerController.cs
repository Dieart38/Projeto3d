using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator anim;
    [Header("Config Player")]
    public float movementSpeed = 5.0f;

    private Vector3 direction;
    private bool isWalk = false;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


        // Get input from keyboard
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
            
        }
        

        direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

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

        anim.SetBool("isWalk", isWalk);
    }
}
