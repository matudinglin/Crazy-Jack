using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    private Vector3 moveTemp;

    private float maxSpeed = 15.0f;
    private float xDifference;
    private float yDifference;
    private float movementThreshold = 2f;

    [SerializeField] private float defaultSize = 13.2672f;
    [SerializeField] private float zoomSize;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        player = gameManager.player;
    }

    // Update is called once per frame
    void Update()
    {

        /*xDifference = player.transform.position.x >= transform.position.x ? calculateDifference(player.transform.position.x, transform.position.x) : 
                            calculateDifference(transform.position.x, player.transform.position.x);

        yDifference = player.transform.position.y >= transform.position.y ? calculateDifference(player.transform.position.y, transform.position.y) :
                            calculateDifference(transform.position.y, player.transform.position.y);

        // auto adjust move speed
        float speed = Mathf.Sqrt(xDifference * xDifference + yDifference * yDifference);
        speed = Mathf.Min(speed, maxSpeed);

        if (xDifference >= movementThreshold)
        {
            moveTemp.x = player.transform.position.x;
            moveTemp.z = -10;
            transform.position = Vector3.MoveTowards(transform.position, moveTemp, speed * Time.deltaTime);
        }


        if (yDifference >= movementThreshold)
        {
            moveTemp.y = player.transform.position.y;
            moveTemp.z = -10;
            transform.position = Vector3.MoveTowards(transform.position, moveTemp, speed * Time.deltaTime);
        }*/
        transform.position = player.transform.position + new Vector3(0, 0, -10);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<Camera>().orthographicSize = zoomSize;
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<Camera>().orthographicSize = defaultSize;
        }
    }

    /*
        Calculate difference between two float value.
        @param val1
        First element.
        @param val2 
        Second element.
        @return val1-val2
        return the difference generated by minus val2 from val1
    */
    private float calculateDifference(float val1, float val2){
        return val1 - val2;
    }

}