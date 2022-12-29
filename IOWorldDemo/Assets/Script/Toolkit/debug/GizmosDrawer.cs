using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosDrawer : MonoBehaviour
{

    public Color color = Color.red;
    public Color rigidbodyColor = Color.green;
    public bool draw = true;
    public bool drawObject = true;
    public bool drawRigidbody = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnDrawGizmos()
    {

        if(!draw) {
            return;
        }

        if(drawRigidbody) {
           // Get the box collider
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if(collider != null) {
                Vector2 size = collider.size;
                Vector2 offset = collider.offset;
                Vector2 position = (Vector2)transform.position + offset;
                Gizmos.color = rigidbodyColor;
                Gizmos.DrawWireCube(position, size);
            }
            
        }

        if(drawObject) {
            Gizmos.color = color;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }

       
    
    }
}
