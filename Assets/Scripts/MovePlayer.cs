using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)){
            Vector2 playerPosition = transform.position;
            playerPosition.y += 0.2f;
            transform.position = playerPosition;
        } else if (Input.GetKey(KeyCode.S)){
            Vector2 playerPosition = transform.position;
            playerPosition.y -= 0.2f;
            transform.position = playerPosition;
        } else if (Input.GetKey(KeyCode.A)){
            Vector2 playerPosition = transform.position;
            playerPosition.x -= 0.2f;
            transform.position = playerPosition;
        } else if (Input.GetKey(KeyCode.D)){
            Vector2 playerPosition = transform.position;
            playerPosition.x += 0.2f;
            transform.position = playerPosition;
        }
    }
}
