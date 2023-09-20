using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] float distanceBetween = 0.2f;
    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 18;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();


    [SerializeField] private List<GameObject> snakeBody = new List<GameObject>();
    [SerializeField] private PlayerTouchMovement touchScreenMovement;
    private float countUp = 0;

    private Rigidbody2D snakeHeadRigidbody;


    private void Start()
    {
        snakeHeadRigidbody = snakeBody[0].GetComponent<Rigidbody2D>();
        CreateBodyParts();
    }

    private void FixedUpdate()
    {
        ManageSnakeBody();
        SnakeMovement(touchScreenMovement.MovementAmount.x, touchScreenMovement.MovementAmount.y);
    }

    private void ManageSnakeBody()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }

        for (int i = 0; i < snakeBody.Count; i++)
        {
            if (snakeBody[i] == null)
            {
                snakeBody.RemoveAt(i);
                i = i - 1;
            }
        }

        if (snakeBody.Count == 0)
        {
            Destroy(this);
            //end the game
        }
    }

    public void SnakeMovement(float horizontalInput, float verticalInput)
    {
        //moves the head with inputs
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        snakeHeadRigidbody.velocity = speed * direction;

        //snakeHeadRigidbody.velocity = speed * snakeBody[0].transform.right;
        //if (Input.GetAxis("Horizontal") != 0)
        //{
        //    snakeBody[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Input.GetAxis("Horizontal")));
        //}

        if (snakeBody.Count > 1)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    private void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            //if (!temp1.GetComponent<MarkerManager>())
            //{
            //    temp1.AddComponent<MarkerManager>();
            //}
            //if (!temp1.GetComponent<Rigidbody2D>())
            //{
            //    temp1.AddComponent<Rigidbody2D>();
            //    temp1.GetComponent<Rigidbody2D>().gravityScale = 0;
            //}
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }

        MarkerManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();

        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;

        if (countUp >= distanceBetween)
        {
            //your snake body parts need marker managers and Rigidbody 2d components
            GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            //if (!temp.GetComponent<MarkerManager>())
            //{
            //    temp.AddComponent<MarkerManager>();
            //}
            //if (!temp.GetComponent<Rigidbody2D>())
            //{
            //    temp.AddComponent<Rigidbody2D>();
            //    temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            //}
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }

    public void AddBodyParts(GameObject obj)
    {
        bodyParts.Add(obj);
    }
}
