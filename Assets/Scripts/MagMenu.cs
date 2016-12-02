using UnityEngine;

public class MagMenu : MonoBehaviour {

    public int distance;
    private bool isBirdsEye = false;
    private Vector3 position;

    void OnEnable()
    {
        LeftMenuController.OnMenuSelection += EnableBirdsEye;
    }

    void OnDisable()
    {
        LeftMenuController.OnMenuSelection -= EnableBirdsEye;
    }

    public void EnableBirdsEye(LeftMenuController.MenuOption selection)
    {
        if(selection != LeftMenuController.MenuOption.Mag_Selected)
        {
            return;
        }

        if (!isBirdsEye)
        {
            zoomOut();
            isBirdsEye = !isBirdsEye;
        }
        else
        {
            zoomIn();
            isBirdsEye = !isBirdsEye;
        }
    }

    private void zoomOut()
    {
        position = this.transform.position;
        Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y + distance, this.transform.position.z);
        this.transform.position = newPosition;
    }

    private void zoomIn()
    {
        this.transform.position = position;
    }
}
