using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("플레이어 연결")]
    [SerializeField] Transform player;
    [Tooltip("카메라 이동 속도")]
    [SerializeField] float smoothing = 0.2f;
    [Tooltip("카메라 경계 설정 - CameraBoundary object 연결")]
    [SerializeField] Transform minCameraBoundary;
    [SerializeField] Transform maxCameraBoundary;

    void Start()
    {
        if(player == null)
        {
            Debug.LogError("CameraController에 player assign해주세요");
            //player = GameObject.FindWithTag("Player").transform;

            Debug.LogError("CameraController에 CameraBoundary assign해주세요.");
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.position.x, maxCameraBoundary.position.x);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}
