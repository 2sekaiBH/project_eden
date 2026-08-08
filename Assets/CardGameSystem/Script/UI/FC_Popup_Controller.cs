using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FC_Popup_Controller : MonoBehaviour
{
    [Header("Choice Cards")]
    [SerializeField] private FinalChoice archiChoice;
    [SerializeField] private FinalChoice eveChoice;
    [SerializeField] private FinalChoice noaChoice;

    [Header("Bottom Select Button")]
    [SerializeField] private Button selectButton;

    private FinalChoice selectedCard;

    public bool HasSelectedCard => selectedCard != null;

    public void ShowPopup()
    {
        Debug.Log("[FC Popup] ShowPopup 호출됨");

        gameObject.SetActive(true);
        OpenPopup();
    }

    public void OpenPopup()
    {
        selectedCard = null;

        // 히든엔딩 조건 확인
        bool noaHiddenEndingUnlocked = CheckNoaAffinityCondition() && CheckHiddenEndingItemCondition();

        archiChoice.Initialize(this, true);
        eveChoice.Initialize(this, true);
        noaChoice.Initialize(this, noaHiddenEndingUnlocked);

        // 아무것도 선택되지 않았으므로 처음에는 버튼 비활성화
        selectButton.interactable = false;
    }

    public void SelectCard(FinalChoice clickedCard)
    {
        // 혹시 잠긴 카드가 외부에서 호출되어도 방어
        if (!clickedCard.IsUnlocked)
            return;

        // 기존 선택 카드가 있으면 선택 해제
        if (selectedCard != null)
        {
            selectedCard.SetSelected(false);
        }

        // 새 카드 선택 확정
        selectedCard = clickedCard;
        selectedCard.SetSelected(true);

        // 카드가 하나 선택되었으므로 선택 버튼 활성화
        selectButton.interactable = true;
    }

    public void OnClickSelectButton()
    {
        if (selectedCard == null)
        {
            return;
        }

        if (GameState.Instance == null)
        {
            Debug.LogError(
                "[FC Popup] GameState가 없어 선택한 진영을 저장할 수 없습니다."
            );
            return;
        }

        switch (selectedCard.CharacterId)
        {
            case "Archi":
                GameState.Instance.SetSelectedFaction(FactionType.Archi);
                Debug.Log("Archi 진영 선택");
                break;

            case "Eve":
                GameState.Instance.SetSelectedFaction(FactionType.Eve);
                Debug.Log("Eve 진영 선택");
                break;

            case "Noa":
                GameState.Instance.SetSelectedFaction(FactionType.Noa);
                GameState.Instance.SetSelectedEnding(EndingType.Exodus); // 히든엔딩 직결
                Debug.Log("Noa 히든 진영 선택");

                SceneManager.LoadScene("05_EndingScene");
                break;

            default:
                Debug.LogError(
                    $"[FC Popup] 알 수 없는 캐릭터 ID: " +
                    $"{selectedCard.CharacterId}"
                );
                return;
        }

        // 여기서 카드게임 씬을 로드하거나,
        // 이후 진행 로직을 호출하면 됨.
        //
        // SceneManager.LoadScene("Lahee_CardGame");
    }

    private bool CheckNoaAffinityCondition()
    {
        // GameState가 생성되지 않았거나 접근할 수 없으면
        // 히든 엔딩 조건 미충족으로 처리
        if (GameState.Instance == null)
        {
            Debug.LogWarning("GameState가 없어 Noa 호감도를 확인할 수 없습니다.");
            return false;
        }

        int noaAffinity = GameState.Instance.GetAffinity("noah");

        Debug.Log($"Noa 호감도 검사: {noaAffinity} / 필요 호감도: 5");

        return noaAffinity >= 5;
    }

    private bool CheckHiddenEndingItemCondition()
    {
        if (GameState.Instance == null)
        {
            Debug.LogWarning(
                "[FC Popup] GameState가 없어 히든 아이템 조건을 확인할 수 없습니다."
            );

            return false;
        }

        int itemCount =
            GameState.Instance.AcquiredHiddenEndingItemCount;

        bool hasAllItems =
            GameState.Instance.HasAllHiddenEndingItems();

        Debug.Log(
            $"[FC Popup] 히든 엔딩 아이템: {itemCount}/5, " +
            $"조건 충족 여부: {hasAllItems}"
        );

        return hasAllItems;
    }

}
