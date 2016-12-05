using UnityEngine;
using System.Collections;
using System;
using VRTK;
using System.Collections.Generic;

public class Resize : MonoBehaviour {

    Vector3 x1PrevPos, x2PrevPos, y1PrevPos, z1PrevPos, z2PrevPos;
    Transform cube;
    private bool firstTime = true;
    private bool wasMoved = false;
    private bool firstTriggerPull = true;

    GameObject cloneObj;

    void OnEnable()
    {
        EventHandlerRightController.OnTriggerClick += EventHandlerRightController_OnTriggerClick;
        EventHandlerRightController.OnTriggerRelease += EventHandlerRightController_OnTriggerRelease;
    }

    void OnDisable()
    {
        EventHandlerRightController.OnTriggerClick -= EventHandlerRightController_OnTriggerClick;
        EventHandlerRightController.OnTriggerRelease += EventHandlerRightController_OnTriggerRelease;
    }

    private void EventHandlerRightController_OnTriggerClick(object sender, ControllerInteractionEventArgs e)
    {
        
    }

    private void EventHandlerRightController_OnTriggerRelease(object sender, ControllerInteractionEventArgs e)
    {
        firstTriggerPull = true;
    }

    void FixedUpdate () {

        if (firstTime)
        {
            cube = getChildByName("Cube").transform;
            x1PrevPos = getChildByName("X1 Sphere").transform.position;
            x2PrevPos = getChildByName("X2 Sphere").transform.position;
            y1PrevPos = getChildByName("Y Sphere").transform.position;
            z1PrevPos = getChildByName("Z1 Sphere").transform.position;
            z2PrevPos = getChildByName("Z2 Sphere").transform.position;

            firstTime = false;
        }

        Vector3 x1Pos = getChildByName("X1 Sphere").transform.position;
        Vector3 x2Pos = getChildByName("X2 Sphere").transform.position;
        Vector3 y1Pos = getChildByName("Y Sphere").transform.position;
        Vector3 z1Pos = getChildByName("Z1 Sphere").transform.position;
        Vector3 z2Pos = getChildByName("Z2 Sphere").transform.position;

        

        if (x1Pos.x != x1PrevPos.x)
        {
            if (firstTriggerPull)
            {
                GameObject cloneObj = Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
                cloneObj.SetActive(false);
                firstTriggerPull = false;
            }
            float difference = x1Pos.x - x1PrevPos.x;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x + difference, cube.transform.localScale.y, cube.transform.localScale.z);
            cube.transform.position = new Vector3(cube.transform.position.x + difference / 2, cube.transform.position.y, cube.transform.position.z);
            x1PrevPos = x1Pos;
            wasMoved = true;
        }

        if (x2Pos.x != x2PrevPos.x)
        {
            if (firstTriggerPull)
            {
                GameObject cloneObj = Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
                cloneObj.SetActive(false);
                firstTriggerPull = false;
            }
            float difference = x2PrevPos.x - x2Pos.x;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x + difference, cube.transform.localScale.y, cube.transform.localScale.z);
            cube.transform.position = new Vector3(cube.transform.position.x - difference / 2, cube.transform.position.y, cube.transform.position.z);
            x2PrevPos = x2Pos;
            wasMoved = true;
        }

        if (z1Pos.z != z1PrevPos.z)
        {
            if (firstTriggerPull)
            {
                GameObject cloneObj = Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
                cloneObj.SetActive(false);
                firstTriggerPull = false;
            }
            float difference = z1Pos.z - z1PrevPos.z;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y, cube.transform.localScale.z + difference);
            cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y, cube.transform.position.z + difference / 2);
            z1PrevPos = z1Pos;
            wasMoved = true;
        }

        if (z2Pos.z != z2PrevPos.z)
        {
            if (firstTriggerPull)
            {
                GameObject cloneObj = Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
                cloneObj.SetActive(false);
                firstTriggerPull = false;
            }
            float difference = z2PrevPos.z - z2Pos.z;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y, cube.transform.localScale.z + difference);
            cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y, cube.transform.position.z - difference / 2);
            z2PrevPos = z2Pos;
            wasMoved = true;
        }

        if (y1Pos.y != y1PrevPos.y)
        {
            if (firstTriggerPull)
            {
                GameObject cloneObj = Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
                cloneObj.SetActive(false);
                firstTriggerPull = false;
            }
            float difference = y1Pos.y - y1PrevPos.y;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y + difference, cube.transform.localScale.z);
            cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y + difference / 2, cube.transform.position.z);
            y1PrevPos = y1Pos;
            wasMoved = true;
        }
        else
        {
            if (wasMoved && firstTriggerPull)
            {
                wasMoved = false;
                GameObject[] list = new GameObject[] { cloneObj, transform.gameObject };
                RedoUndo.AddUndo(list);
            }
        }
    }

    private GameObject getChildByName(string name)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).transform.name == name)
            {
                return this.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }
}
