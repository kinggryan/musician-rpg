﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCamera : MonoBehaviour
{
    public float transitionTime = .5f;
    private Camera camera;
    private float cameraWidth;
    private float cameraHeight;
    private Vector2 cameraPosition;
    private GameObject player;

    public float leftBoundary;
    public float rightBoundary;
    public float topBoundary;
    public float bottomBoundary;
    bool movingCameraLeft;
    bool movingCameraRight;
    bool movingCameraUp;
    bool movingCameraDown;
    float newXPosition;

    // float cameraLeftBoundary(){
    //     float leftBoundary = cameraWidth/2 - cameraPosition.x;
    //     return boundary;
    // }

    // float cameraRightBoundary(){
    //     float boundary = cameraWidth/2 + cameraPosition.x;
    //     return boundary;
    // }

    // float cameraTopBoundary(){
    //     float boundary = cameraHeight/2 + cameraPosition.y;
    //     return boundary;
    // }

    // float cameraBottomBoundary(){
    //     float boundary = cameraHeight/2 - cameraPosition.y;
    //     return boundary;
    // }

    bool playerOutsideOfLeftBoundary(){
        if(player.transform.position.x < leftBoundary){
            return true;
        }else{
            return false;
        }
    }

    bool playerOutsideOfRightBoundary(){
        if(player.transform.position.x > rightBoundary){
            return true;
        }else{
            return false;
        }
    }

    bool playerOutsideOfTopBoundary(){
        if(player.transform.position.y > topBoundary){
            return true;
        }else{
            return false;
        }
    }

    bool playerOutsideOfBottomBoundary(){
        if(player.transform.position.y < bottomBoundary){
            return true;
        }else{
            return false;
        }
    }

    void GetCameraBoundaries(){
        cameraPosition = transform.position;
        leftBoundary = cameraPosition.x - cameraWidth/2;
        rightBoundary = cameraPosition.x + cameraWidth/2 ;
        topBoundary = cameraPosition.y + cameraHeight/2;
        bottomBoundary = cameraPosition.y - cameraHeight/2;
    }

    void Start()
    {
        player = Object.FindObjectOfType<PlayerMovementController>().gameObject;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        camera = GetComponent<Camera>();
        cameraPosition = transform.position;
        cameraWidth = camera.pixelWidth/32;
        cameraHeight = camera.pixelHeight/32;
        
        GetCameraBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerOutsideOfLeftBoundary()){
            Debug.Log("player outside of left boundary");
            moveCameraLeft();
        }else if(playerOutsideOfRightBoundary()){
            Debug.Log("player outside of right boundary");
            moveCameraRight();
        }else if(playerOutsideOfTopBoundary()){
            Debug.Log("player outside of top boundary");
            moveCameraUp();
        }else if(playerOutsideOfBottomBoundary()){
            Debug.Log("player outside of bottom boundary");
            moveCameraDown();
        }

        if(movingCameraRight){
            
            if(transform.position.x >= newXPosition){
                Debug.Log("Done moving camera right");
                movingCameraRight = false;
                GetCameraBoundaries();
            }else{
                Debug.Log("MovingCameraRight");
                transform.position = new Vector3(transform.position.x + Time.deltaTime/transitionTime, transform.position.y, transform.position.z);
            }
        }
    }

    IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end)
        {
            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            while (elapsedTime < transitionTime)
            {
                objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / transitionTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = end;
            GetCameraBoundaries();
        }

    void moveCameraLeft(){
        Vector3 newPosition = new Vector3(leftBoundary - cameraWidth/2, transform.position.y, transform.position.z);
        StartCoroutine(MoveOverSeconds(camera.gameObject, newPosition));
    }

    void moveCameraRight(){
        Vector3 newPosition = new Vector3(rightBoundary + cameraWidth/2, transform.position.y, transform.position.z);
        StartCoroutine(MoveOverSeconds(camera.gameObject, newPosition));
                
    }
    void moveCameraUp(){
        Vector3 newPosition = new Vector3(transform.position.x, topBoundary + cameraHeight/2, transform.position.z);
        StartCoroutine(MoveOverSeconds(camera.gameObject, newPosition));
                
    }
    void moveCameraDown(){
        Vector3 newPosition = new Vector3(transform.position.x, bottomBoundary - cameraHeight/2, transform.position.z);
        StartCoroutine(MoveOverSeconds(camera.gameObject, newPosition));
                
    }
}
