using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] Vector3 moveVal;
    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        this.moveSpeed = 10f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += moveVal * moveSpeed * Time.deltaTime;
    }

    void OnRestart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void MoveLeft(){
       
    }

    void MoveRight(){
        
    }

    void jump(){
        
    }

}
