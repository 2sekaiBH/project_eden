using System.Collections;
using UnityEngine;

public class EndingDialogueStarter : MonoBehaviour
{
    [Header("Ending Dialogue Controller")]
    [SerializeField] private IntroDialogueController dialogueController;

    private IEnumerator Start()
    {
        // IntroDialogueController의 Start()가 먼저 끝나도록
        // 한 프레임 기다린 뒤 엔딩 대화를 시작한다.
        //
        // autoStartOnStart를 꺼 둔 상태에서 사용한다.
        yield return null;

        if (dialogueController == null)
        {
            Debug.LogError(
                "[EndingDialogueStarter] IntroDialogueController가 연결되지 않았습니다."
            );
            yield break;
        }

        if (GameState.Instance == null)
        {
            Debug.LogError(
                "[EndingDialogueStarter] GameState가 없어 엔딩 결과를 읽을 수 없습니다."
            );
            yield break;
        }

        EndingType endingType = GameState.Instance.SelectedEnding;

        string startNodeId = GetStartNodeId(endingType);

        if (string.IsNullOrEmpty(startNodeId))
        {
            Debug.LogError(
                $"[EndingDialogueStarter] 시작할 엔딩이 없습니다. " +
                $"현재 엔딩 값: {endingType}"
            );
            yield break;
        }

        Debug.Log(
            $"[EndingDialogueStarter] 엔딩 재생 시작 | " +
            $"Type: {endingType}, Start ID: {startNodeId}"
        );

        dialogueController.StartDialogue(startNodeId);
    }

    private string GetStartNodeId(EndingType endingType)
    {
        switch (endingType)
        {
            case EndingType.GameOver:
                return "ending_gameover_001";

            case EndingType.Offline:
                return "ending_offline_001";

            case EndingType.Reconnect:
                return "ending_reconnect_001";

            case EndingType.Exodus:
                return "ending_exodus_001";

            default:
                return null;
        }
    }
}
