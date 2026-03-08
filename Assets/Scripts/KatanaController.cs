using UnityEngine;

public class KatanaController : MonoBehaviour
{
    public Katana katana; 
    public Animator playerAnimator;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetBool("KatanaActive", true);
            katana.SpecialAttack();
        }
    }

}
