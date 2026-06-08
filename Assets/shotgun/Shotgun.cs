using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.GraphicsBuffer;

public class Shotgun : MonoBehaviour
{
    public GameObject shotgun;
    public Animator shotgunAnimator;
    [SerializeField] private LayerMask visionMask;
    [SerializeField]  private Transform firePoint;
    [SerializeField] private Transform crosshair;
    [SerializeField] private Animator playerAnimator; //za katana napad
    [SerializeField] private GameObject bulletSpritePrefab;
    private bool wasShotgunEquipped = false;

    public AudioClip shotgun_shoot_sound;
    public AudioClip shotgun_reload_sound;
    private AudioSource audioSource;

    
    public int ammo = 7;
 
    private void Start()
    {

        shotgun.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

    }
    void Update()
    {
        Katana();

        Prikaz();
        Idle();
        Pucanje();
        Reload();
    }

    void Prikaz()
    {
        if (playerAnimator.GetBool("KatanaActive"))
        {
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.Alpha1) || scroll > 0f)
        {
           
            if (ammo>3)
            {
                shotgun.SetActive(true);
                shotgunAnimator.SetTrigger("EquipBlue");
            }
            else
            {
                
                    shotgun.SetActive(true);
                    shotgunAnimator.SetTrigger("EquipRed");
                
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha0) || scroll < 0f)
        {
            shotgun.SetActive(false);
        }

    }

    void Pucanje()
    {
        if (!shotgun.activeSelf)
            return;

        if (ammo == 0)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(shotgun_shoot_sound);

            Vector2 dir = (crosshair.position - firePoint.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(
                firePoint.position,
                dir,
                Mathf.Infinity,
                visionMask
            );

            
            if (ammo > 3)
                shotgunAnimator.SetTrigger("ShootBlue");
            else
                shotgunAnimator.SetTrigger("ShootRed");

            ammo--;

            
            Vector2 targetPoint;

            if (hit.collider != null)
            {
                Debug.Log("Pogodak: " + hit.collider.name);

                targetPoint = hit.point;

                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(25);
                }

                BatHealth bat = hit.collider.GetComponentInParent<BatHealth>();
                if (bat != null)
                    bat.TakeDamage(25);
            }
            else
            {
                
                targetPoint = (Vector2)firePoint.position + dir * 20f;
            }

            
            GameObject bullet = Instantiate(
                bulletSpritePrefab,
                firePoint.position,
                Quaternion.identity
            );

            
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            bullet.GetComponent<BulletVisual>().Init(targetPoint);
        }
    }

    void Reload()
    {
        if (!shotgun.activeSelf)
            {
                return;
            }
        if (Input.GetKeyDown(KeyCode.R) && ammo <= 3)
        {
            audioSource.PlayOneShot(shotgun_reload_sound, 0.7f); 
            shotgunAnimator.SetTrigger("Reload");
            ammo = 7;
        }
    }

    void Idle()
    {
        if (!shotgun.activeSelf)
        {
            return;
        }

        if (ammo > 3)
        {
            shotgunAnimator.SetInteger("AmmoState", 1);
        }

        else
        {
            shotgunAnimator.SetInteger("AmmoState", 0);
        }
    }

    private void Katana() 
    {
        bool katanaActive = playerAnimator.GetBool("KatanaActive");

        if (katanaActive)
        {
            if (shotgun.activeSelf)
            {
                wasShotgunEquipped = true;
                shotgun.SetActive(false);
            }

            return;
        }
        else
        {
            if (wasShotgunEquipped && !shotgun.activeSelf)
            {
                shotgun.SetActive(true);

                if (ammo > 3)
                    shotgunAnimator.SetTrigger("EquipBlue");
                else
                    shotgunAnimator.SetTrigger("EquipRed");

                wasShotgunEquipped = false;
            }
        }

    }

}
