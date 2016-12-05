using UnityEngine;
using System.Collections;
using System;

public class BuildingMenu : MonoBehaviour {

    private Vector3[] coordinates;
    private int coordCount;
    private bool isBuilding = false;

    public GameObject buildingPrefab;

    private enum acceptableMenus { Mag_Selected };

    void OnEnable()
    {
        LeftMenuController.OnMenuSelection += EnableBuildingMode;
        EventHandlerLeftController.OnTriggerClick += EventHandlerLeftController_OnTriggerClick;
    }

    void OnDisable()
    {
        LeftMenuController.OnMenuSelection -= EnableBuildingMode;
        EventHandlerLeftController.OnTriggerClick -= EventHandlerLeftController_OnTriggerClick;
    }

    private void EnableBuildingMode(LeftMenuController.MenuOption selection)
    {     
        if (selection != LeftMenuController.MenuOption.Building_Selected)
        {
            if(!isAcceptable(selection))
                isBuilding = false;
            return;
        }

        coordinates = new Vector3[3];
        coordCount = 0;

        if (!isBuilding)
        {
            isBuilding = !isBuilding;
        }
        else
        {
            isBuilding = !isBuilding;
        }
    }

    private void EventHandlerLeftController_OnTriggerClick(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        if(!isBuilding)
        {
            return;
        }

        if (CubeCollision.globalIsStrongHighlighted)
        {
            return;
        }

        Vector3 controllerCoords = this.getChildByName("Controller (left)").transform.position;
        coordinates[coordCount++] = controllerCoords;

        if(coordCount >= 3)
        {
            CreateBuilding();
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

    private void CreateBuilding()
    {
        float[] minCoords = new float[3];
        float[] maxCoords = new float[3];

        minCoords[0] = Math.Min(coordinates[0].x, Math.Min(coordinates[1].x, coordinates[2].x));
        maxCoords[0] = Math.Max(coordinates[0].x, Math.Max(coordinates[1].x, coordinates[2].x));
        minCoords[1] = Math.Min(coordinates[0].y, Math.Min(coordinates[1].y, coordinates[2].y));
        maxCoords[1] = Math.Max(coordinates[0].y, Math.Max(coordinates[1].y, coordinates[2].y));
        minCoords[2] = Math.Min(coordinates[0].z, Math.Min(coordinates[1].z, coordinates[2].z));
        maxCoords[2] = Math.Max(coordinates[0].z, Math.Max(coordinates[1].z, coordinates[2].z));

        float xScale = maxCoords[0] - minCoords[0];
        float yScale = maxCoords[1] - minCoords[1];
        float zScale = maxCoords[2] - minCoords[2];

        Vector3 position = new Vector3(minCoords[0] + xScale/2, minCoords[1] + yScale / 2, minCoords[2] + zScale / 2);
        Vector3 localScale = new Vector3(xScale, yScale, zScale);

        GameObject gameObject = Instantiate(buildingPrefab, position, Quaternion.identity) as GameObject;

        
        var childCube = gameObject.transform.FindChild("Cube");

        childCube.transform.localScale = localScale;

        gameObject.transform.Find("X1 Sphere").transform.position = childCube.position + new Vector3( childCube.localScale.x/2 + 1, 0, 0);
        gameObject.transform.Find("X2 Sphere").transform.position = childCube.position - new Vector3(childCube.localScale.x / 2 + 1, 0, 0);
        gameObject.transform.Find("Y Sphere").transform.position = childCube.position + new Vector3(0, childCube.localScale.y / 2 + 1, 0);
        gameObject.transform.Find("Z1 Sphere").transform.position = childCube.position + new Vector3(0, 0, childCube.localScale.z / 2 + 1);
        gameObject.transform.Find("Z2 Sphere").transform.position = childCube.position - new Vector3(0, 0, childCube.localScale.z / 2 + 1);

        RedoUndo.AddUndo(new GameObject[]{ null, gameObject});

        coordinates = new Vector3[3];
        coordCount = 0;
    }

    private bool isAcceptable(LeftMenuController.MenuOption selection)
    {
        try
        {
            acceptableMenus option = (acceptableMenus)Enum.Parse(typeof(acceptableMenus), selection.ToString());
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
