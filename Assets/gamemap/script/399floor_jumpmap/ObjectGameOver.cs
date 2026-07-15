using UnityEngine;

public class GameOver : MonoBehaviour
{
    private static bool isGameOverTriggered = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOverTriggered) return;

        // 닿은 오브젝트의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            isGameOverTriggered = true;

            Debug.Log("게임오버");

            // 게임오버를 관리하는 매니저에게 게임오버 씬을 띄우라고 명령
            
        }
    }
}
