using UnityEngine;

public class KatanaController : MonoBehaviour
{
    public Katana katana; 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            katana.SpecialAttack();
        }
    }

}
