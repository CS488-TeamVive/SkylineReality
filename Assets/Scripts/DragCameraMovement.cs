using UnityEngine;
using VRTK;

public class DragCameraMovement : MonoBehaviour {

    public float dragSpeed;
    private bool dragEnabled = false;
    private Vector3 prevControllerPos = new Vector3(0, 0, 0);

    void OnEnable()
    {
        EventHandlerRightController.OnGripPress += EnableDrag;
        EventHandlerRightController.OnGripRelease += DisableDrag;
    }

    void OnDisable()
    {
        EventHandlerRightController.OnGripPress -= EnableDrag;
        EventHandlerRightController.OnGripRelease -= DisableDrag;
    }

    void FixedUpdate()
    {
        if (!dragEnabled)
        {
            return;
        }

        Vector3 controllerCoords = getChildByName("Controller (right)").transform.position;

        if(controllerCoords == null)
        {
            return;
        }

        Vector3 controllerMovement = prevControllerPos - controllerCoords;
        Vector3 cameraMovement = controllerMovement * dragSpeed * Time.deltaTime;
        prevControllerPos = controllerCoords + cameraMovement;
        transform.position += cameraMovement;
    }

    private GameObject getChildByName(string name)
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            if(this.transform.GetChild(i).transform.name == name)
            {
                return this.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    private void EnableDrag(object sender, ControllerInteractionEventArgs e)
    {
        dragEnabled = true;
        Vector3 controllerCoords = getChildByName("Controller (right)").transform.position;
        prevControllerPos = controllerCoords;
    }

    private void DisableDrag(object sender, ControllerInteractionEventArgs e)
    {
        dragEnabled = false;
    }
}
