using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PasswordUI : MonoBehaviour
{
    public static PasswordUI Instance { get; private set; }

    [Header("UI 컴포넌트 연결")]
    [SerializeField] private GameObject passwordPanel;

    [Tooltip("자물쇠의 각 자릿수를 나타내는 배열")]
    [SerializeField] private TextMeshProUGUI[] digitTexts;

    [Header("색상 연출")]
    [SerializeField] private Color activeDigitColor = new Color(59f/255f, 255f/255f, 255f/255f); // 현재 선택 중인 자릿수 색상
    [SerializeField] private Color normalDigitColor = Color.white;  // 대기 중인 자릿수 색상

    [Header("비밀번호 설정")]
    [SerializeField] private string correctPassword = "5321"; // 정답 비밀번호
    [Header("성공 이벤트")]
    [SerializeField] private UnityEvent onPasswordSuccess;

    private int[] currentDigits;  // 각 자릿수의 현재 숫자
    private int currentFocusIndex = 0; // 현재 조종 중인 자릿수 인덱스
    private bool isOpen = false;  // UI가 켜져있는지 여부

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (digitTexts != null && digitTexts.Length > 0)
        {
            currentDigits = new int[digitTexts.Length];
        }

        if (passwordPanel != null)
        {
            passwordPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isOpen) return;


        // 1. 위쪽 방향키 (숫자 증가 0 -> 9 -> 0)
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
        {
            ChangeDigitValue(1);
        }
        // 2. 아래쪽 방향키 (숫자 감소 0 -> 9 -> 0)
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
        {
            ChangeDigitValue(-1);
        }
        // 3. 오른쪽 방향키 (다음 자릿수로 이동)
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            ConfirmCurrentDigit();
        }
        // 4. 왼쪽 방향키 (이전 자릿수로 되돌아가기)
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            MoveToPreviousDigit();
        }
        // 5. 엔터키 (즉시 정답 검증)
        else if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            CheckPassword();
        }
        // 6. esc키 (UI 닫기)
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ClosePasswordUI();
        }
    }

    // 비밀번호 UI 열기
    public void OpenPasswordUI()
    {
        isOpen = true;
        currentFocusIndex = 0;

        // 모든 자릿수 숫자 0으로 초기화
        for (int i = 0; i < currentDigits.Length; i++)
        {
            currentDigits[i] = 0;
        }

        if (passwordPanel != null) passwordPanel.SetActive(true);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(ESfx.tallcase_O); ;
        }
        Time.timeScale = 0f; // 게임 일시정지

        UpdateUI();
    }

    // 비밀번호 UI 닫기
    public void ClosePasswordUI()
    {
        isOpen = false;
        if (passwordPanel != null) passwordPanel.SetActive(false);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(ESfx.tallcase_C);
        }
        Time.timeScale = 1f;
    }

    private void ChangeDigitValue(int amount)
    {
        currentDigits[currentFocusIndex] += amount;

        // 0~9 다이얼 순환 처리 (9에서 올리면 0, 0에서 내리면 9)
        if (currentDigits[currentFocusIndex] > 9) currentDigits[currentFocusIndex] = 0;
        if (currentDigits[currentFocusIndex] < 0) currentDigits[currentFocusIndex] = 9;

        UpdateUI();
    }

    // 다음 자릿수로
    private void ConfirmCurrentDigit()
    {
        // 아직 마지막 자릿수가 아니라면 -> 다음 자릿수로 이동
        if (currentFocusIndex < digitTexts.Length - 1)
        {
            currentFocusIndex++;
            UpdateUI();
        }
        // 마지막 자릿수에서 엔터를 누르면 정답 검사
        else
        {
            CheckPassword();
        }
    }

    // 이전 자릿수로 돌아가기
    private void MoveToPreviousDigit()
    {
        if (currentFocusIndex > 0)
        {
            currentFocusIndex--;
            UpdateUI();
        }
    }

    // 정답 검사
    private void CheckPassword()
    {
        // 현재 입력된 숫자 배열을 하나의 문자열로 변환
        string inputResult = string.Join("", currentDigits);

        if (inputResult == correctPassword)
        {
            Debug.Log("해제 성공");

            ClosePasswordUI();

            onPasswordSuccess?.Invoke();
        }
        else
        {
            Debug.Log("비밀번호가 틀렸습니다. 다시 시도하세요.");
            // 틀렸을 경우 첫 번째 자릿수로 돌려보내고 초기화
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(ESfx.error_glitch);
            }
            currentFocusIndex = 0;
            for (int i = 0; i < currentDigits.Length; i++) currentDigits[i] = 0;
            UpdateUI();
        }
    }

    // 화면 UI 및 색상 갱신
    private void UpdateUI()
    {
        for (int i = 0; i < digitTexts.Length; i++)
        {
            if (digitTexts[i] == null) continue;

            // 숫자 텍스트 변경
            digitTexts[i].text = currentDigits[i].ToString();

            if (i == currentFocusIndex)
            {
                digitTexts[i].color = activeDigitColor;
            }
            else
            {
                digitTexts[i].color = normalDigitColor;
            }
        }
    }
}