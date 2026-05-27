using UnityEngine;
public class WeaponRotation : MonoBehaviour
{
    [SerializeField] private Transform crosshair;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private float aimOffset = 0f;

    void Awake()
    {
        if (crosshair == null)
        {
            GameObject ch = GameObject.FindGameObjectWithTag("Crosshair");
            if (ch != null) crosshair = ch.transform;
            else Debug.LogWarning("WeaponRotation: Crosshair nije pronaðen!");
        }
    }

    void Update()
    {
        if (crosshair == null) return;
        Vector2 dir = (crosshair.position - weaponPivot.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float sign = Mathf.Sign(weaponPivot.lossyScale.x);
        if (sign < 0f)
            angle = 180f + angle;
        weaponPivot.rotation = Quaternion.AngleAxis(angle + aimOffset, Vector3.forward);
    }
}