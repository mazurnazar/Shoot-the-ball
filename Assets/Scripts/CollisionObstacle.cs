using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObstacle : MonoBehaviour
{
    private Manager manager;
    private float alpha;
    private int touchCount = 0;
    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }
    //when bullet collides with obstacle or door
    private void OnCollisionEnter(Collision collision)
    {
        Renderer renderer = collision.gameObject.GetComponent<Renderer>();
        if (collision.gameObject.tag == "Obstacle")
        {
            alpha = 1;
            touchCount++;
            manager.CanShoot = true;
            renderer.material.color = Color.yellow;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<SphereCollider>().isTrigger = true;
            GetComponent<SphereCollider>().radius = 2;
            StartCoroutine(DestroyObstacles(collision.gameObject));

        }
        if (collision.gameObject.tag == "Door")
        {
            gameObject.SetActive(false);
        }
    }
    // after collide and changing ist sphere collider radius and becoming trigger it collides with more obstacles
    private void OnTriggerEnter(Collider other)
    {
        Renderer renderer = other.GetComponent<Renderer>();
        if (other.tag == "Obstacle")
        {
            touchCount++;
            other.GetComponent<Renderer>().material.color = Color.yellow;
            manager.CanShoot = true;
            StartCoroutine(DestroyObstacles(other.gameObject));
        }
        
    }
    
    // Destroy obstacles by making its color.a changing to zero
    IEnumerator DestroyObstacles(GameObject obstacle)
    {
        Renderer renderer = obstacle.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.a = alpha;

        renderer.material.color = color;
        alpha -= 0.05f;

        yield return new WaitForSeconds(0.1f);

        if (alpha > 0)
        {
            StartCoroutine(DestroyObstacles(obstacle));
        }
        else 
        {
            Destroy(obstacle);
            touchCount--;
            if (touchCount == 0)
               gameObject.SetActive(false);
        }
       

    }
    
}
