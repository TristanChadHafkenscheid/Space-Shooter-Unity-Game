using UnityEngine;

public class AddBodyDynamic : MonoBehaviour
{
    [SerializeField] private GameObject obj1;
    [SerializeField] private GameObject obj2;
    [SerializeField] private GameObject obj3;
    [SerializeField] private GameObject obj4;

    private SnakeManager snakeM;

    private void Start()
    {
        snakeM = GetComponent<SnakeManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
            snakeM.AddBodyParts(obj1);
        if (Input.GetKeyDown("2"))
            snakeM.AddBodyParts(obj2);
        if (Input.GetKeyDown("3"))
            snakeM.AddBodyParts(obj3);
        if (Input.GetKeyDown("4"))
            snakeM.AddBodyParts(obj4);
    }
}
