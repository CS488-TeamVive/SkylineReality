using UnityEngine;
using System.Collections;
using System;
using VRTK;
using System.Collections.Generic;

public class Resize : MonoBehaviour {

    Vector3 x1PrevPos, x2PrevPos, y1PrevPos, z1PrevPos, z2PrevPos;
    Transform cube;
    private bool firstTime = true;

    GameObject cloneObj; //= Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
    //cloneObj.SetActive(false);

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
        List<GameObject> list = new List<GameObject>();
        list.Add(getChildByName("X1 Sphere").transform.gameObject);
        list.Add(getChildByName("X2 Sphere").transform.gameObject);
        list.Add(getChildByName("Y Sphere").transform.gameObject);
        list.Add(getChildByName("Z1 Sphere").transform.gameObject);
        list.Add(getChildByName("Z2 Sphere").transform.gameObject);

        foreach(GameObject child in list)
        {
            SphereColliderDragXDir x = (SphereColliderDragXDir)child.GetComponent(typeof(SphereColliderDragXDir));
            SphereColliderDragYDir y = (SphereColliderDragYDir)child.GetComponent(typeof(SphereColliderDragYDir));
            SphereColliderDragZDir z = (SphereColliderDragZDir)child.GetComponent(typeof(SphereColliderDragZDir));

            if(x != null)
            {
                if (x.IsGrabbed())
                {
                    GameObject cloneObj = Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
                    cloneObj.SetActive(false);
                }
            }
            else if(y != null)
            {
                if (y.IsGrabbed())
                {
                    GameObject cloneObj = Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
                    cloneObj.SetActive(false);
                }
            }
            else if(z != null)
            {
                if (z.IsGrabbed())
                {
                    GameObject cloneObj = Instantiate(transform.gameObject, transform.position, Quaternion.identity) as GameObject;
                    cloneObj.SetActive(false);
                }
            }
        }
    }

    private void EventHandlerRightController_OnTriggerRelease(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.Log(cloneObj);
        /*GameObject[] list = new GameObject[] { cloneObj, transform.gameObject };
        RedoUndo.AddUndo(list);
        */
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
            float difference = x1Pos.x - x1PrevPos.x;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x + difference, cube.transform.localScale.y, cube.transform.localScale.z);
            cube.transform.position = new Vector3(cube.transform.position.x + difference / 2, cube.transform.position.y, cube.transform.position.z);
            x1PrevPos = x1Pos;
        }

        if (x2Pos.x != x2PrevPos.x)
        {
            float difference = x2PrevPos.x - x2Pos.x;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x + difference, cube.transform.localScale.y, cube.transform.localScale.z);
            cube.transform.position = new Vector3(cube.transform.position.x - difference / 2, cube.transform.position.y, cube.transform.position.z);
            x2PrevPos = x2Pos;
        }

        if (z1Pos.z != z1PrevPos.z)
        {
            float difference = z1Pos.z - z1PrevPos.z;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y, cube.transform.localScale.z + difference);
            cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y, cube.transform.position.z + difference / 2);
            z1PrevPos = z1Pos;
        }

        if (z2Pos.z != z2PrevPos.z)
        {
            float difference = z2PrevPos.z - z2Pos.z;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y, cube.transform.localScale.z + difference);
            cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y, cube.transform.position.z - difference / 2);
            z2PrevPos = z2Pos;
        }

        if (y1Pos.y != y1PrevPos.y)
        {
            float difference = y1Pos.y - y1PrevPos.y;
            cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y + difference, cube.transform.localScale.z);
            cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y + difference / 2, cube.transform.position.z);
            y1PrevPos = y1Pos;
        }
        else
        {
            return;
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
