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
       
        Prikaz();
        Idle();
        Pucanje();
        Reload();
    }

    void Prikaz()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(ammo>3)
            {
                shotgun.SetActive(true);
                shotgunAnimator.SetTrigger("EquipBlue");
            }
            else
            {
                {
                    shotgun.SetActive(true);
                    shotgunAnimator.SetTrigger("EquipRed");
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            shotgun.SetActive(false);
        }

    }

    void Pucanje()
    {
      
        if(!shotgun.activeSelf)
        {
            return;
        }

        if (ammo == 0)
        {
            return;
        }

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
            {
                shotgunAnimator.SetTrigger("ShootBlue");
            }
            else
            {
                shotgunAnimator.SetTrigger("ShootRed");
            }

            ammo--;

            if (hit.collider != null)
            {
                
                Debug.Log("Pogodak: " + hit.collider.name);
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(25); 
                }

                
            }

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
            audioSource.PlayOneShot(shotgun_reload_sound, 0.7f); //smanjivanje glasnoæe na 70%
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


    
}
