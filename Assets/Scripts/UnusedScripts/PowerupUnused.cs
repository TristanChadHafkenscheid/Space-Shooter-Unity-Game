using UnityEngine;
using Player;

public class PowerupUnused : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField] //0 equals triple shot, 1 = speed, 2 = shields, 3 = big laser
    private int powerupID;
    [SerializeField]
    private AudioClip _audioClip;

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (powerupID == 3) //big laser
        {
            transform.Rotate(10 * Time.deltaTime * Vector3.forward);
        }

        if (transform.position.y < -4.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_audioClip, transform.position);
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
                    case 3:
                        player.BigLaserActive();
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
