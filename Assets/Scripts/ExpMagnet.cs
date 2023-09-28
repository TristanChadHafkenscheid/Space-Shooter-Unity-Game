using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpMagnet : MonoBehaviour
{
    private Player _playerController;
    [SerializeField] private float _magnetSpeed;
    [SerializeField] private float _closeToPlayer;


    void Start()
    {
        _playerController = Player.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exp"))
        {
            Debug.Log("collided");
            StartCoroutine(MoveExp(collision.transform));
        }
    }

    IEnumerator MoveExp(Transform expTransform)
    {
        while (Vector3.Distance(expTransform.position, transform.position) >= _closeToPlayer)
        {
            expTransform.position = Vector3.MoveTowards(expTransform.position,
                _playerController.transform.position, _magnetSpeed * Time.deltaTime);
            yield return null;
        }
        //collected exp
    }
}
