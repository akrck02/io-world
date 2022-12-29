using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAnimationController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer
            body,
            eyes,
            tail1,
            tail2;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private float horizontal;
    
    public float speed = .6f;

    /**
    * Update is called once per frame
    */
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (horizontal > 0)
        {
            MoveAnimation(true);
        }
        else if (horizontal < 0)
        {
            MoveAnimation(false);
        }
    }

    /**
    * On restart input is detected
    * restarts current scene
    */
    public void Restart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    /**
    * Makes the player jump
    */
    public void JumpSound()
    {
       
        jumpSound.Play();

    }

    /**
    * Moves the player
    */
    public void Move(float x)
    {
        horizontal = x;
    }

    /**
    * Performs move animation
    */
    void MoveAnimation(bool right)
    {
        float eyesX = eyes.transform.position.x;
        float tail1X = tail1.transform.position.x;
        float tail2X = tail2.transform.position.x;


        if (right)
        {
            eyesX += .1f * speed / 2;
            tail1X -= .05f * speed;
            tail2X -= .1f * speed;
        }
        else
        {
            eyesX -= .1f * speed / 2;
            tail1X += .05f * speed;
            tail2X += .1f * speed;
        }

        if (eyesX > body.transform.position.x + .07f)  
            eyesX = body.transform.position.x + .07f;

        if (tail1X < body.transform.position.x -.1f) 
            tail1X = body.transform.position.x -.1f;

        if (tail2X < body.transform.position.x -.2f) 
            tail2X = body.transform.position.x -.2f;

        if (eyesX < body.transform.position.x - .07f) 
            eyesX = body.transform.position.x - .07f;

        if (tail1X > body.transform.position.x +.1f) 
            tail1X = body.transform.position.x +.1f;

        if (tail2X > body.transform.position.x +.2f) 
            tail2X = body.transform.position.x +.2f;

        Vector3 eyesTarget =
            new Vector3(eyesX,
                eyes.transform.position.y,
                body.transform.position.z - 1);
        Vector3 Tail1Target =
            new Vector3(tail1X,
                tail1.transform.position.y,
                body.transform.position.z);
        Vector3 Tail2Target =
            new Vector3(tail2X,
                tail2.transform.position.y,
                body.transform.position.z);

        eyes.transform.position =
            Vector2
                .MoveTowards(eyes.transform.position,
                eyesTarget,
                1 * Time.deltaTime);

        tail1.transform.position =
            Vector2
                .MoveTowards(tail1.transform.position,
                Tail1Target,
                1 * Time.deltaTime);

        tail2.transform.position =
            Vector2
                .MoveTowards(tail2.transform.position,
                Tail2Target,
                1 * Time.deltaTime);
    }

    /**
    * On Collision is detected with the end of the level
    * loads the next scene
    */
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "end")
        {
            levelLoader.LoadNextLevel();
        }
    }

}
