using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject door;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform edgeDoor;

    private Rigidbody playerRb;

    private bool canJump;
    private bool isGrounded = true;

    private float speed = 0.5f;
    private float jumpForce = 7f;
    private float minPlayerRadius = 0.3f;
    private float distanceToObstacle = 2f;
    private float distanceOpenDoor = 5f;

    private float playerRadius;
    public float PlayerRadius { get => playerRadius; set { playerRadius = value; } }

    private void Start()
    {
        playerRadius = 1f;
        playerRb = GetComponent<Rigidbody>();
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
        // spherecast 100 forward to check if theres obstacles, door, finish or other and on its distance to them decide to move player further
        if (Physics.SphereCast(transform.position, playerRadius / 2f, fwd, out hit, 100))
        {
            string name = hit.transform.gameObject.tag;
            switch (name)
            {
                case "Obstacle":
                    if (hit.distance > distanceToObstacle)
                    {
                         canJump = true;
                         Move();
                    }
                    else canJump = false;
                    break;
                case "Door":
                    canJump = true;
                    Move();
                    if (hit.distance < distanceOpenDoor )
                    {
                        OpenDoor();
                    }
                    break;
                case "Untagged":
                    break;
                case "Finish":
                    canJump = true;
                    Move();
                    break;
            }
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        // if collides with finish game won
        if (collision.gameObject.tag == "Finish")
        {
            manager.GameOver("WIN");
            manager.IsPlaying = false;
        }
        // if collides with road is grounded true so player can jump agaain
        if (collision.gameObject.tag == "Road")
        {
            isGrounded = true;
        }
    }
    // when player radius is too small game is lost
    void Lost()
    {
        if (playerRadius < minPlayerRadius)
        {
            manager.CanShoot = false;
            manager.IsPlaying = false;
            manager.GameOver("Lost");
        }
    }
    // open door 
    void OpenDoor()
    {
        door.transform.RotateAround(edgeDoor.position, Vector3.up, 90);
    }
    // moving forward and jumping of player
    void Move()
    {
        if (manager.IsPlaying)
        {
            if (isGrounded && canJump)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                transform.position += (new Vector3(0, 0, speed)) * Time.deltaTime;
                if(camera.transform.position.z < 20)
                camera.transform.position += (new Vector3(0, 0, speed)) * Time.deltaTime;
            }
            isGrounded = false;
        }
    }
}
