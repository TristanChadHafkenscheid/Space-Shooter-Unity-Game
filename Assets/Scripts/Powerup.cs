using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField] //0 equals triple shot, 1 = speed, 2 = shields
    private int powerupID;

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -4.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
