using UnityEngine;
using UnityEngine.EventSystems;

public class HoverMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float moveAmount = 10f;
    [SerializeField] private float speed = 5f;

    private Vector3 startPos;
    private Vector3 targetPos;

    private void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos;
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPos = startPos + Vector3.left * moveAmount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPos = startPos;
    }
}