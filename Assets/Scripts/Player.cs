using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject door;
    public Transform point;
    private bool doorOpen;

    private float playerRadius;
    public float PlayerRadius { get => playerRadius; set { playerRadius = value; } }

    private void Start()
    {
        playerRadius = 1f;
    }
    void Update()
    {
        CheckObstacles();
        Lost();
    }
    void CheckObstacles()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, playerRadius/2f, fwd, out hit, 100))
        {
            string name = hit.transform.gameObject.tag;
            switch (name)
            {
                case "Obstacle":
                    if (hit.distance > 2f)
                    {
                        transform.position += new Vector3(0, 0, 0.1f);
                    }
                    break;
                case "Door":
                    transform.position += new Vector3(0, 0, 0.1f);
                    if (hit.distance < 5f && !doorOpen)
                    {
                        OpenDoor();
                    }
                    break;
                case "Untagged":
                    break;
                case "Finish":
                    transform.position += new Vector3(0, 0, 0.1f);
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("WIN");
        }
    }
    void Lost()
    {
        if (playerRadius < 0.3f)
        {
            manager.CanShoot = false;
            Debug.Log("Lost");
        }
    }
    void OpenDoor()
    {
        door.transform.RotateAround(point.position, Vector3.up, 90);
        doorOpen = true;

    }
}
