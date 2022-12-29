using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct FrameInput {
    public float X;
    public bool JumpDown;
    public bool JumpUp;
}

public class CharacterController : MonoBehaviour
{


    private PlayerAnimationController animationController;

    #region Movement
    [Header("MOVEMENT")] [SerializeField]
    public float speed = 5f;
    public float jumpForce = 5f;
    public float jumpTime = 0.2f;

    private Rigidbody2D rigidbody;

    private FrameInput Input;
    
    private float lastJumpPressed = 0f;
    private float lastJump = 0f;

    private float lastGrounded = 0f;
    private bool isGrounded = false;
    
    private bool doubleJump = false;
    
    
    public float momentum = 0f;
    public bool hasMomentum = false;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        interactIcon.SetActive(false);
        holder = transform.Find("Holder");
        animationController = GetComponent<PlayerAnimationController>();

        Input = new FrameInput {
            JumpDown = false,
            JumpUp = false,
            X = 0
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Do not rotate
        rigidbody.freezeRotation = true;

        // Gather input
        Input.X = UnityEngine.Input.GetAxisRaw("Horizontal");

        // if the player is grounded, reset double jump
        if(isGrounded){
            doubleJump = true;
        }

        // if holding something do not show interact icon
        if(holder.childCount > 0){
            interactIcon.SetActive(false);
        }

        checkGrounded();

        
    }
    private void shortJump(){
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * .5f);
    } 

    private void checkGrounded(){
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, .75f, LayerMask.GetMask("Ground"));
    }

    private void move(float x) {
      
            // Calculate the movement
            float xMovement = 0;

            if (x != 0) {
                xMovement = x * speed;
            } else {
                xMovement = 0;
            }    

            animationController.Move(xMovement);

            // move the player
            rigidbody.velocity = new Vector2(xMovement, rigidbody.velocity.y);

            // if the player has momentum, add it to the velocity
            if(hasMomentum) {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x + momentum, rigidbody.velocity.y);
            }
        
    }

    public void OnMove(InputAction.CallbackContext context) {
        
    
        Debug.Log("Moving");

        // Get vector from input
        Vector2 input = context.ReadValue<Vector2>();
        move(input.x);

    }


    public void OnJump(InputAction.CallbackContext context) {

       
        if (context.performed) {
            lastJumpPressed = Time.time;
            Input.JumpDown = true;

            // if the player is grounded and the jump button is pressed within the jump time
            if (isGrounded && Time.time - lastJumpPressed < jumpTime) {
                Debug.Log("Jumping");
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                lastJump = Time.time;
                animationController.JumpSound();
                return;
            }

            // if double jump is available and the jump button is pressed within the jump time
            if(doubleJump && Time.time - lastJumpPressed < jumpTime){
                Debug.Log("Double Jumping");
                doubleJump = false;
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                lastJump = Time.time;
                animationController.JumpSound();
                return;                 
            }


        }
        else if (context.canceled) {
            Input.JumpUp = true;
        }

        
    }


        #region Interaction
        [Header("INTERACTION")] [SerializeField]
        private GameObject interactIcon;
        public Transform holder;
        private Vector2 boxSize = new Vector2(1.938797f, 3.20f);

        public void OpenInteactableIcon(){
            interactIcon.SetActive(true);
        }

        public void CloseInteactableIcon(){
            interactIcon.SetActive(false);
        }

        private void CheckInteraction(){

            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(10f, 10f), 0, Vector2.zero);
            if(hits.Length > 0) {
                foreach (RaycastHit2D hit in hits) {
                    if (hit.IsInteractable()) {

                        hit.Interact();
                        return;
                    }
                }
            }
        }
    
               
        public void Interact(InputAction.CallbackContext context) {
               
            if(context.performed) {
                CheckInteraction();
            } 
        }
        #endregion



}
