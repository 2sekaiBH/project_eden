using UnityEngine;
using UnityEngine.UI;

public class ItemAcquisitionUI : MonoBehaviour
{
    public static ItemAcquisitionUI Instance { get; private set; }

    [System.Serializable]
    public struct ItemUIData
    {
        public int id;             // 1: 의문의 캔, 2: 전단지, 3: 고물 로봇, 4: 안내 칩
        public string itemName;    // 아이템 이름
        [TextArea(3, 5)]
        public string itemDesc;
        public Sprite itemIcon;    // 아이템 이미지 스프라이트
    }

    [Header("아이템 데이터베이스")]
    [SerializeField] private ItemUIData[] itemDatabase;

    [Header("UI 컴포넌트 연결")]
    [SerializeField] private GameObject popupPanel;       // UI 패널 부모 오브젝트
    [SerializeField] private Image iconImage;             // 아이템 아이콘 이미지 칸
    [SerializeField] private Text itemInfoText; // "[아이템이름]을(를) 획득했습니다!" 텍스트 칸
    [SerializeField] private Text itemDescText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (popupPanel != null) popupPanel.SetActive(false); // 게임 시작 시엔 꺼둠
    }

    // ID로 데이터베이스를 조회하는 함수
    private ItemUIData FindItemDataByID(int id)
    {
        foreach (var data in itemDatabase)
        {
            if (data.id == id) return data;
        }
        return new ItemUIData { id = id, itemName = "미지의 아이템", itemDesc = "정체를 알 수 없다.", itemIcon = null };
    }

    // 팝업창 열기 (InteractableObject가 호출함)
    public void ShowAcquisitionPopup(int id)
    {
        ItemUIData targetData = FindItemDataByID(id);

        // UI 정보 갈아끼우기
        if (itemInfoText != null)
            itemInfoText.text = $"[{targetData.itemName}]";

        if (itemDescText != null)
            itemDescText.text = targetData.itemDesc;

        if (iconImage != null)
        {
            if (targetData.itemIcon != null)
            {
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = targetData.itemIcon;
                iconImage.SetNativeSize(); // 아이콘 이미지의 원본 크기로 조정
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }
        }

        // 팝업창 활성화
        if (popupPanel != null) popupPanel.SetActive(true);

        Time.timeScale = 0f; 
    }

    // ★ [확인] 버튼에 연결할 패널 닫기 함수
    public void OnClickConfirmButton()
    {
        if (popupPanel != null) popupPanel.SetActive(false);

        Time.timeScale = 1f; 
    }
}