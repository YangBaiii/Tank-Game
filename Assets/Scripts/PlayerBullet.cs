using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float bulletSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
        if (transform.position.x < -5 || transform.position.x > 20 || transform.position.y < -20 || transform.position.y > 20)
        {
            Destroy(gameObject);
        }
    }
}
