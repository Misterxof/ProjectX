using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter " + collision.ToString());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       // Debug.Log("Stay");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D with " + collision.gameObject.name);
        
        gameObject.GetComponent<CraterCreator>().OnCollision(collision, collision.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        //transform.parent.gameObject.GetComponent<CraterCreator>().OnCollision(collision);
    }
}
