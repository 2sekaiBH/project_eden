using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] Transform minCameraBoundary;
    [SerializeField] Transform maxCameraBoundary;

    void Start()
    {
        if(player == null)
        {
            Debug.LogWarning("CameraController에 player assign해주세요");
            player = GameObject.FindWithTag("Player").transform;

            Debug.LogWarning("CameraController에 CameraBoundary assign해주세요.");
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.position.x, maxCameraBoundary.position.x);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}
