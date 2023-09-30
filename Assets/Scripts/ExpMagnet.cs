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
        Debug.Log("collided");
        if (collision.CompareTag("Exp"))
        {
            StartCoroutine(MoveExp(collision.gameObject));
        }
    }

    IEnumerator MoveExp(GameObject expGameObject)
    {
        while (Vector3.Distance(expGameObject.transform.position, transform.position) >= _closeToPlayer)
        {
            expGameObject.transform.position = Vector3.MoveTowards(expGameObject.transform.position,
                _playerController.transform.position, _magnetSpeed * Time.deltaTime);
            yield return null;
        }
        expGameObject.SetActive(false);
        //collected exp
    }
}
