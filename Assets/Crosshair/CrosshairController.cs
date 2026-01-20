using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        // Sakrij fizièki miš
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

        // Naði kameru
        cam = Camera.main;
    }

    void Update()
    {
        if (cam == null) return;

      
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        
        transform.position = mousePos;
    }
}
