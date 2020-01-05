using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform player;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = Object.FindObjectOfType<PlayerMovementController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 player2Dposition = new Vector3(player.position.x,player.position.y,transform.position.z);

        transform.position = Vector3.Lerp(transform.position, player2Dposition + offset, Time.deltaTime * speed);   
    }
}
