using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject bulletPref;
   
    [SerializeField] private GameObject bullet;

    [SerializeField] private Text resultText;
    [SerializeField] private GameObject repeatPanel;

    private float bulletRadius;
    private float currentPlayerRadius;
    private float currentBulletRadius;
    private bool canShoot = true;
    public bool CanShoot { get => canShoot; set { canShoot = value; } }

    private bool isPlaying = true;

    public bool IsPlaying { get => isPlaying; set { isPlaying = value; } }


    void Start()
    {
        bulletRadius = 0.2f;
    }

    void Update()
    {
        MousePress();
    }
    // method for mouse press
    void MousePress()
    {
        // first press, "create" bullet and change radius of player
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            RestoreBullet();
            currentBulletRadius = bullet.transform.localScale.x;
            player.transform.localScale = new Vector3(currentPlayerRadius, currentPlayerRadius, currentPlayerRadius);
            player.PlayerRadius = RadiusCalculate();
        }
        // when mouse is continuous pressed and player radius is bigger than bullet 
        if (Input.GetMouseButton(0) && canShoot && player.PlayerRadius >= currentBulletRadius)
        {
            bullet.transform.localScale *= 1.05f;
            currentBulletRadius = bullet.transform.localScale.x;
            bullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * 0.1f, ForceMode.Force);
            currentPlayerRadius = RadiusCalculate();
            player.transform.localScale = new Vector3(currentPlayerRadius, currentPlayerRadius, currentPlayerRadius);
        }
        // when mouse is released
        if (Input.GetMouseButtonUp(0))
        {
            bullet.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);
            player.PlayerRadius = currentPlayerRadius;
            canShoot = false;
        }
    }   
    // recalculation radius of player depending on current radius of bullet
    float RadiusCalculate()
    {
        if (player.PlayerRadius >= currentBulletRadius)
        {
            currentPlayerRadius = Mathf.Pow(Mathf.Pow(player.PlayerRadius, 3) - Mathf.Pow(currentBulletRadius, 3), 1/3f);
            road.transform.localScale = new Vector3(currentPlayerRadius, road.transform.localScale.y, road.transform.localScale.z);
        }
        return currentPlayerRadius;
    }
    // restore initial parameters of bullet, such as position, scale and so on
    void RestoreBullet()
    {
        bullet.transform.position = player.transform.position + new Vector3(0, 0, 1f);
        bullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        bullet.GetComponent<Rigidbody>().isKinematic = false;
        bullet.GetComponent<SphereCollider>().isTrigger = false;
        bullet.GetComponent<SphereCollider>().radius = 0.5f;
        bullet.SetActive(true);
    }    
    // activates repeat panel and win/lost text
    public void GameOver(string result)
    {
        repeatPanel.SetActive(true);
        resultText.text = result;
    }
    //reloads scene
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}