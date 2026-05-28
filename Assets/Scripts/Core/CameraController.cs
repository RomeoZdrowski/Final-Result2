using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Player")]
    [SerializeField] private Transform player;

    [Header("Horizontal")]
    [SerializeField] private float aheadDistance = 3f;
    [SerializeField] private float cameraSpeed = 10f;

    [Header("Vertical")]
    [SerializeField] private float verticalOffset = 1f;
    [SerializeField] private float verticalSpeed = 5f;

    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.2f;

    private float lookAhead;
    private float currentPosX;
    private float currentPosY;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        // Горизонталь — lookahead в сторону движения игрока
        float targetX = player.position.x + lookAhead;
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);

        // Вертикаль — плавно тянется за Y игрока
        float targetY = Mathf.Lerp(currentPosY, player.position.y + verticalOffset, Time.deltaTime * verticalSpeed);
        currentPosY = targetY;

        // Применяем позицию через SmoothDamp для общей мягкости
        Vector3 targetPos = new Vector3(targetX, currentPosY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
        currentPosY = _newRoom.position.y;
    }
}