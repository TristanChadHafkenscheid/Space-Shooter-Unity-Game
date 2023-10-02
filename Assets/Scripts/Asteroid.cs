using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 20.0f;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float startLerpDuration;
    [SerializeField] private Vector3 startOffScreenPos;
    [SerializeField] private Vector3 startOnScreenPos;

    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
        StartCoroutine(MoveToStartPosition(startLerpDuration, startOffScreenPos, startOnScreenPos));
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerLaser"))
        {
            Instantiate(_explosionPrefab, transform.localPosition, Quaternion.identity);
            Destroy(other.gameObject);
            //_spawnManager.StartSpawning();
            Destroy(gameObject, 0.25f);
        }
    }

    IEnumerator MoveToStartPosition(float lerpDuration, Vector3 offScreenPos, Vector3 onScreenPos)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            transform.localPosition = Vector3.Lerp(offScreenPos, onScreenPos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = onScreenPos;
    }
}
