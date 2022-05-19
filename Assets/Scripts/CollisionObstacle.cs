using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObstacle : MonoBehaviour
{
    private Manager manager;
    private float alpha;
    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            alpha = 1;
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
          //  Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Renderer renderer = other.GetComponent<Renderer>();
        if (other.tag == "Obstacle")
        {
            other.GetComponent<Renderer>().material.color = Color.yellow;
            manager.CanShoot = true;
            StartCoroutine(DestroyObstacles(other.gameObject));
        }
        
    }
    
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
        }
       

    }
    
}
