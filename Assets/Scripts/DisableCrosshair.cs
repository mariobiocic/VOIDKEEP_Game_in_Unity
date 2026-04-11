using UnityEngine;

public class DisableCrosshair : MonoBehaviour
{
    void Start()
    {
        GameObject cross = GameObject.Find("Crosshair");
        if (cross != null)
            cross.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}