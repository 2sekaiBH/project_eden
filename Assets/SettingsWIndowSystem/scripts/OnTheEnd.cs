using System.Collections;
using UnityEngine;

public class OnTheEnd : MonoBehaviour
{

    public void OnStartTheEnd()
    {
        StartCoroutine(OnQuitButtonClicked());
    }

    public IEnumerator OnQuitButtonClicked()
    {
        yield return new WaitForSecondsRealtime(7f);
#if UNITY_EDITOR
        // 유니티 에디터 실행 중일 때는 플레이 모드를 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 실제 빌드된 게임(exe 등)에서는 게임 창을 완전히 종료
        Application.Quit();
#endif
    }

}
