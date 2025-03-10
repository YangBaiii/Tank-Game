using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 7f;

    private Vector2 playPosition;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playPosition = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Euler(0,0,90);
            playPosition.y += playerSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0,0,180);
            playPosition.x -= playerSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Euler(0,0,270);
            playPosition.y -= playerSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            playPosition.x += playerSpeed * Time.deltaTime;
        }
        transform.position = playPosition;
        ShootBullet();
    }

    void ShootBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
        }
    }
}
