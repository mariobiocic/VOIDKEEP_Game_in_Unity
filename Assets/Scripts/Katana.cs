using UnityEngine;

public class Katana : MonoBehaviour
{
    public GameObject Sword;
    public Animator SwordAnimator;

    public GameObject player;
    public GameObject hitbox;

    private Collider2D playerCollider;
    private Collider2D hitboxCollider;

    void Start()
    {
        playerCollider = player.GetComponent<Collider2D>();
        hitboxCollider = hitbox.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(playerCollider, hitboxCollider);

        Sword.SetActive(false);
    }

    public void SpecialAttack()
    {
        Sword.SetActive(true);
        SwordAnimator.Play("Sword_attack_special");
    }

    public void DisableSword()
    {
        Sword.SetActive(false);
    }
}
