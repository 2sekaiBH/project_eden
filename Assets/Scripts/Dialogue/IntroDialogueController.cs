using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;

public class IntroDialogueController : MonoBehaviour
{
    [Serializable]
    private class SpriteEntry
    {
        public string key;
        public Sprite sprite;
    }

    [Serializable]
    private class AudioEntry
    {
        public string key;
        public AudioClip clip;
    }

    [Header("Dialogue JSON")]
    [SerializeField] private TextAsset dialogueJson;

    [Header("Dialogue Flow")]
    [SerializeField] private bool autoStartOnStart = true;
    [SerializeField] private UnityEvent onDialogueFinished;

    [Header("Screens")]
    [SerializeField] private GameObject prologueScreen;
    [SerializeField] private GameObject nameInputScreen;
    [SerializeField] private GameObject characterDialogueScreen;

    [Header("Shared Background")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private CanvasGroup prologueDim;
    [SerializeField] private CanvasGroup talkDim;

    [Header("Prologue UI")]
    [SerializeField] private TMP_Text prologueText;
    [SerializeField] private GameObject prologueNextIndicator;
    [SerializeField] private Button prologueNextButton;

    [Header("Name Input UI")]
    [SerializeField] private TMP_Text namePromptText;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button nameConfirmButton;
    [SerializeField] private TMP_Text nameErrorText;

    [Header("Game Start UI")]
    [SerializeField] private GameObject gameStartScreen;
    [SerializeField] private TMP_Text gameStartText;

    [SerializeField, Min(0f)]
    private float gameStartDuration = 1.5f;

    [Header("Dialogue UI")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialogueNextHint;
    [SerializeField] private Button dialogueNextButton;

    [Header("Choice UI")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TMP_Text[] choiceTexts;


    [Header("Character UI")]
    [SerializeField] private GameObject leftCharacterGroup;
    [SerializeField] private Image leftCharacterImage;
    [SerializeField] private GameObject rightCharacterGroup;
    [SerializeField] private Image rightCharacterImage;

    [Header("Asset Key Mappings")]
    [SerializeField] private SpriteEntry[] backgrounds;
    [SerializeField] private SpriteEntry[] characters;
    [SerializeField] private AudioEntry[] soundEffects;
    [SerializeField] private AudioSource soundEffectSource;

    [Header("Text Transition")]
    [SerializeField, Min(0f)]
    private float textFadeDuration = 0.2f;

    private readonly Dictionary<string, DialogueNode> nodeById = new();
    private readonly Dictionary<string, Sprite> backgroundByKey = new();
    private readonly Dictionary<string, Sprite> characterByKey = new();
    private readonly Dictionary<string, AudioClip> soundByKey = new();

    private DialogueNode currentNode;
    private bool isTransitioning;

    private void Awake()
    {
        if (!ValidateRequiredReferences())
        {
            enabled = false;
            return;
        }

        BuildAssetDictionaries();
        LoadDialogue();

        nameConfirmButton.onClick.AddListener(ConfirmName);
        nameInputField.onSubmit.AddListener(_ => ConfirmName());
        nameInputField.onValueChanged.AddListener(UpdateNameButtonState);

        choicePanel.SetActive(false);
    }

    private void Start()
    {
        if (autoStartOnStart)
        {
            StartDialogue(GetStartId());
        }
        else
        {
            SetOnlyScreen(null);
        }
    }

    private void Update()
    {
        if (isTransitioning || currentNode == null)
        {
            return;
        }

        // 이름 입력 중에는 좌클릭이나 Space로 넘어가지 않음
        if (currentNode.type == "nameInput" ||
            currentNode.type == "choice")
        {
            return;
        }

        bool spacePressed =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame;

        bool leftClickPressed =
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame;

        if (spacePressed || leftClickPressed)
        {
            Advance();
        }
    }

    private void OnDestroy()
    {
        if (nameConfirmButton != null)
            nameConfirmButton.onClick.RemoveListener(ConfirmName);

        if (nameInputField != null)
        {
            nameInputField.onValueChanged.RemoveListener(
                UpdateNameButtonState
            );
            nameInputField.onSubmit.RemoveAllListeners();
        }
    }

    private bool ValidateRequiredReferences()
    {
        bool valid =
            dialogueJson != null &&
            prologueScreen != null &&
            nameInputScreen != null &&
            gameStartScreen != null &&
            gameStartText != null &&
            characterDialogueScreen != null &&
            backgroundImage != null &&
            prologueDim != null &&
            talkDim != null &&
            prologueText != null &&
            prologueNextIndicator != null &&
            prologueNextButton != null &&
            namePromptText != null &&
            nameInputField != null &&
            nameConfirmButton != null &&
            speakerNameText != null &&
            dialogueText != null &&
            dialogueNextHint != null &&
            dialogueNextButton != null &&
            choicePanel != null &&
            choiceButtons != null &&
            choiceTexts != null &&
            choiceButtons.Length >= 3 &&
            choiceTexts.Length >= 3;

        if (!valid)
        {
            Debug.LogError(
                "IntroDialogueController의 필수 Inspector 연결이 빠져 있습니다."
            );
        }

        return valid;
    }


    private void BuildAssetDictionaries()
    {
        AddSpriteEntries(backgrounds, backgroundByKey, "배경");
        AddSpriteEntries(characters, characterByKey, "캐릭터");

        if (soundEffects != null)
        {
            foreach (AudioEntry entry in soundEffects)
            {
                if (entry == null ||
                    string.IsNullOrWhiteSpace(entry.key) ||
                    entry.clip == null)
                {
                    continue;
                }

                soundByKey[entry.key] = entry.clip;
            }
        }
    }

    private static void AddSpriteEntries(
        SpriteEntry[] entries,
        Dictionary<string, Sprite> target,
        string label)
    {
        if (entries == null)
        {
            return;
        }

        foreach (SpriteEntry entry in entries)
        {
            if (entry == null ||
                string.IsNullOrWhiteSpace(entry.key) ||
                entry.sprite == null)
            {
                continue;
            }

            target[entry.key] = entry.sprite;
        }
    }

    private void LoadDialogue()
    {
        DialogueFile data =
            JsonUtility.FromJson<DialogueFile>(dialogueJson.text);

        if (data == null || data.nodes == null || data.nodes.Length == 0)
        {
            throw new InvalidOperationException(
                "대화 JSON에 nodes가 없거나 JSON 형식이 잘못되었습니다."
            );
        }

        foreach (DialogueNode node in data.nodes)
        {
            if (node == null || string.IsNullOrWhiteSpace(node.id))
            {
                continue;
            }

            if (nodeById.ContainsKey(node.id))
            {
                Debug.LogWarning($"중복 대사 ID를 덮어씁니다: {node.id}");
            }

            nodeById[node.id] = node;
        }

        loadedStartId = data.startId;
    }

    private string loadedStartId;

    private string GetStartId()
    {
        if (string.IsNullOrWhiteSpace(loadedStartId))
        {
            throw new InvalidOperationException(
                "대화 JSON의 startId가 비어 있습니다."
            );
        }

        return loadedStartId;
    }

    public void StartDialogue(string nodeId)
    {
        if (string.IsNullOrWhiteSpace(nodeId))
        {
            Debug.LogError("시작할 대사 ID가 비어 있습니다.");
            return;
        }

        StopAllCoroutines();
        isTransitioning = false;
        HideChoices();
        DisplayNode(nodeId);
    }

    private void DisplayNode(string nodeId)
    {
        if (string.IsNullOrWhiteSpace(nodeId))
        {
            FinishDialogue();
            return;
        }

        if (!nodeById.TryGetValue(nodeId, out DialogueNode node))
        {
            Debug.LogError($"대사 ID를 찾을 수 없습니다: {nodeId}");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(DisplayNodeRoutine(node));
    }

    private IEnumerator DisplayNodeRoutine(DialogueNode node)
    {
        isTransitioning = true;
        currentNode = node;

        ApplyBackground(node.background);
        PlaySoundEffect(node.sfx);

        switch (node.type)
        {
            case "prologue":
            case "narration":
                yield return ShowPrologueNode(node);
                break;

            case "nameInput":
                ShowNameInputNode(node);
                break;

            case "dialogue":
                yield return ShowDialogueNode(node);
                break;

            case "choice":
                yield return ShowChoiceNode(node);
                break;

            default:
                Debug.LogError(
                    $"지원하지 않는 대사 type입니다: {node.type} / {node.id}"
                );
                break;
        }

        isTransitioning = false;
    }

    private IEnumerator ShowPrologueNode(DialogueNode node)
    {
        HideChoices();
        SetOnlyScreen(prologueScreen);
        SetDimVisibility(usePrologueDim: true);

        prologueNextIndicator.SetActive(false);
        prologueNextButton.interactable = false;

        yield return SetDimAlpha(
            prologueDim,
            node.dimAlpha,
            node.fadeDim,
            node.fadeDuration
        );

        yield return ReplaceTextWithFade(
            prologueText,
            ReplacePlayerName(node.text)
        );

        prologueNextIndicator.SetActive(true);
        prologueNextButton.interactable = true;
    }

    private void ShowNameInputNode(DialogueNode node)
    {
        HideChoices();
        SetOnlyScreen(nameInputScreen);
        SetDimVisibility(usePrologueDim: false, hideBoth: true);

        namePromptText.text = ReplacePlayerName(node.text);
        nameInputField.SetTextWithoutNotify("");
        nameInputField.interactable = true;
        nameConfirmButton.interactable = false;

        UpdateNameButtonState("");

        if (nameErrorText != null)
        {
            nameErrorText.text = "";
        }

        nameInputField.ActivateInputField();
    }

    private IEnumerator ShowDialogueNode(DialogueNode node)
    {
        HideChoices();
        SetOnlyScreen(characterDialogueScreen);
        SetDimVisibility(usePrologueDim: false);

        speakerNameText.text = ReplacePlayerName(node.speaker);
        speakerNameText.gameObject.SetActive(
            !string.IsNullOrWhiteSpace(speakerNameText.text)
        );

        dialogueNextHint.SetActive(false);
        dialogueNextButton.interactable = false;

        ApplyCharacter(
            node.leftCharacter,
            leftCharacterGroup,
            leftCharacterImage
        );

        ApplyCharacter(
            node.rightCharacter,
            rightCharacterGroup,
            rightCharacterImage
        );

        yield return SetDimAlpha(
            talkDim,
            node.dimAlpha,
            node.fadeDim,
            node.fadeDuration
        );

        yield return ReplaceTextWithFade(
            dialogueText,
            ReplacePlayerName(node.text)
        );
        dialogueNextHint.SetActive(true);
        dialogueNextButton.interactable = true;
    }

    public void Advance()
    {
        if (isTransitioning ||
            currentNode == null ||
            currentNode.type == "nameInput" ||
            currentNode.type == "choice")
        {
            return;
        }

        DisplayNode(currentNode.nextId);
    }

    public void ConfirmName()
    {
        if (isTransitioning ||
            currentNode == null ||
            currentNode.type != "nameInput")
        {
            return;
        }

        string enteredName =
            nameInputField.text.Trim();

        if (string.IsNullOrWhiteSpace(enteredName))
        {
            nameConfirmButton.interactable = false;

            if (nameErrorText != null)
            {
                nameErrorText.text =
                    "이름을 한 글자 이상 입력해 주세요.";
            }

            nameInputField.ActivateInputField();
            return;
        }

        // Scene이 바뀌어도 유지되는 공용 상태에 이름 저장
        GameState.Instance.SetPlayerName(enteredName);

        // Game Start 화면이 끝난 뒤 이동할 JSON 노드
        string nextNodeId = currentNode.nextId;

        isTransitioning = true;

        nameInputField.interactable = false;
        nameConfirmButton.interactable = false;

        StartCoroutine(
            ShowGameStartThenContinue(nextNodeId)
        );
    }

    private IEnumerator ShowGameStartThenContinue(string nextNodeId)
    {
        SetOnlyScreen(gameStartScreen);

        SetDimVisibility(
            usePrologueDim: false,
            hideBoth: true
        );

        gameStartText.text =
            ReplacePlayerName(
                "%%님,영원한 낙원에 오신 걸 환영합니다."
            );

        SetTextAlpha(gameStartText, 0f);

        yield return FadeTextAlpha(
            gameStartText,
            0f,
            1f,
            0.4f
        );

        yield return new WaitForSecondsRealtime(
            gameStartDuration
        );

        yield return FadeTextAlpha(
            gameStartText,
            1f,
            0f,
            0.3f
        );

        isTransitioning = false;

        DisplayNode(nextNodeId);
    }

    private void UpdateNameButtonState(string inputValue)
    {
        bool hasValidName =
            !string.IsNullOrWhiteSpace(inputValue);

        nameConfirmButton.interactable = hasValidName;

        if (hasValidName && nameErrorText != null)
        {
            nameErrorText.text = "";
        }
    }

    private void ApplyBackground(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        if (backgroundByKey.TryGetValue(key, out Sprite sprite))
        {
            backgroundImage.sprite = sprite;
            return;
        }

        Debug.LogWarning($"배경 키가 Inspector에 등록되지 않았습니다: {key}");
    }

    private void ApplyCharacter(
        string key,
        GameObject group,
        Image targetImage)
    {
        if (group == null || targetImage == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            group.SetActive(false);
            return;
        }

        if (characterByKey.TryGetValue(key, out Sprite sprite))
        {
            targetImage.sprite = sprite;
            group.SetActive(true);
            return;
        }

        Debug.LogWarning($"캐릭터 키가 Inspector에 등록되지 않았습니다: {key}");
        group.SetActive(false);
    }

    private void PlaySoundEffect(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        if (soundEffectSource == null)
        {
            Debug.LogWarning(
                $"효과음 AudioSource가 연결되지 않아 재생하지 못했습니다: {key}"
            );
            return;
        }

        if (soundByKey.TryGetValue(key, out AudioClip clip))
        {
            soundEffectSource.PlayOneShot(clip);
            return;
        }

        Debug.LogWarning($"효과음 키가 Inspector에 등록되지 않았습니다: {key}");
    }

    private IEnumerator SetDimAlpha(
        CanvasGroup target,
        float alpha,
        bool fade,
        float duration)
    {
        if (target == null)
        {
            yield break;
        }

        alpha = Mathf.Clamp01(alpha);

        target.gameObject.SetActive(true);
        target.interactable = false;
        target.blocksRaycasts = false;

        // Image 자체의 Alpha가 0이면 CanvasGroup을 올려도 안 보이므로 보정
        Graphic dimGraphic =
            target.GetComponent<Graphic>();

        if (dimGraphic != null)
        {
            Color graphicColor = dimGraphic.color;
            graphicColor.a = 1f;
            dimGraphic.color = graphicColor;
        }

        if (!fade || duration <= 0f)
        {
            target.alpha = alpha;
            yield break;
        }

        target.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress =
                Mathf.Clamp01(elapsed / duration);

            target.alpha =
                Mathf.Lerp(0f, alpha, progress);

            yield return null;
        }

        target.alpha = alpha;
    }

    private void SetOnlyScreen(GameObject activeScreen)
    {
        prologueScreen.SetActive(
            activeScreen == prologueScreen
        );

        nameInputScreen.SetActive(
            activeScreen == nameInputScreen
        );

        gameStartScreen.SetActive(
            activeScreen == gameStartScreen
        );

        characterDialogueScreen.SetActive(
            activeScreen == characterDialogueScreen
        );
    }

    private void SetDimVisibility(
        bool usePrologueDim,
        bool hideBoth = false)
    {
        if (hideBoth)
        {
            prologueDim.gameObject.SetActive(false);
            talkDim.gameObject.SetActive(false);
            return;
        }

        prologueDim.gameObject.SetActive(usePrologueDim);
        talkDim.gameObject.SetActive(!usePrologueDim);
    }

    private string ReplacePlayerName(string value)
    {
        return GameState.Instance.ReplacePlayerName(value);
    }

    private void FinishDialogue()
    {
        isTransitioning = false;

        prologueNextButton.interactable = false;
        dialogueNextButton.interactable = false;

        HideChoices();
        SetOnlyScreen(null);

        Debug.Log(
            $"현재 대사가 끝났습니다. " +
            $"이름: {GameState.Instance.PlayerName}, " +
            $"아키텍트: {GameState.Instance.GetAffinity("architect")}, " +
            $"카인: {GameState.Instance.GetAffinity("cain")}, " +
            $"노아: {GameState.Instance.GetAffinity("noah")}"
        );

        onDialogueFinished?.Invoke();
    }


    private IEnumerator FadeTextAlpha(
    TMP_Text target,
    float startAlpha,
    float endAlpha,
    float duration)
    {
        if (target == null)
        {
            yield break;
        }

        if (duration <= 0f)
        {
            SetTextAlpha(target, endAlpha);
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress =
                Mathf.Clamp01(elapsed / duration);

            float alpha =
                Mathf.Lerp(startAlpha, endAlpha, progress);

            SetTextAlpha(target, alpha);

            yield return null;
        }

        SetTextAlpha(target, endAlpha);
    }

    private static void SetTextAlpha(
        TMP_Text target,
        float alpha)
    {
        if (target == null)
        {
            return;
        }

        Color color = target.color;
        color.a = Mathf.Clamp01(alpha);
        target.color = color;
    }

    private IEnumerator ReplaceTextWithFade(
    TMP_Text target,
    string newText)
    {
        if (target == null)
        {
            yield break;
        }

        // 기존 텍스트가 있으면 먼저 사라짐
        if (!string.IsNullOrEmpty(target.text))
        {
            yield return FadeTextAlpha(
                target,
                target.color.a,
                0f,
                textFadeDuration * 0.5f
            );
        }

        target.text = newText;
        SetTextAlpha(target, 0f);

        // 새 텍스트가 나타남
        yield return FadeTextAlpha(
            target,
            0f,
            1f,
            textFadeDuration
        );
    }
    private void HideChoices()
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }

        if (choiceButtons == null)
        {
            return;
        }

        foreach (Button button in choiceButtons)
        {
            if (button == null)
            {
                continue;
            }

            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }
    }
    private IEnumerator ShowChoiceNode(DialogueNode node)
    {
        SetOnlyScreen(characterDialogueScreen);
        SetDimVisibility(usePrologueDim: false);

        dialogueNextHint.SetActive(false);
        dialogueNextButton.interactable = false;

        ApplyCharacter(
            node.leftCharacter,
            leftCharacterGroup,
            leftCharacterImage
        );

        ApplyCharacter(
            node.rightCharacter,
            rightCharacterGroup,
            rightCharacterImage
        );

        yield return SetDimAlpha(
            talkDim,
            node.dimAlpha,
            node.fadeDim,
            node.fadeDuration
        );

        string speaker =
            ReplacePlayerName(node.speaker);

        speakerNameText.text = speaker;

        speakerNameText.gameObject.SetActive(
            !string.IsNullOrWhiteSpace(speaker)
        );

        yield return ReplaceTextWithFade(
            dialogueText,
            ReplacePlayerName(node.text)
        );

        ShowChoices(node);
    }

    private void ShowChoices(DialogueNode node)
    {
        if (node.choices == null ||
            node.choices.Length == 0)
        {
            Debug.LogError(
                $"선택지 데이터가 없습니다: {node.id}"
            );

            return;
        }

        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button button = choiceButtons[i];
            TMP_Text text = choiceTexts[i];

            button.onClick.RemoveAllListeners();

            if (i >= node.choices.Length)
            {
                button.gameObject.SetActive(false);
                continue;
            }

            button.gameObject.SetActive(true);
            button.interactable = true;

            ChoiceData choice = node.choices[i];

            text.text =
                ReplacePlayerName(choice.text);

            int capturedIndex = i;

            button.onClick.AddListener(
                () => SelectChoice(capturedIndex)
            );
        }
    }

    private void SelectChoice(int choiceIndex)
    {
        if (isTransitioning ||
            currentNode == null ||
            currentNode.type != "choice" ||
            currentNode.choices == null ||
            choiceIndex < 0 ||
            choiceIndex >= currentNode.choices.Length)
        {
            return;
        }

        isTransitioning = true;

        ChoiceData selectedChoice =
            currentNode.choices[choiceIndex];

        ChangeAffinity(
            selectedChoice.affinityTarget,
            selectedChoice.affinityDelta
        );

        HideChoices();

        string nextNodeId =
            selectedChoice.nextId;

        isTransitioning = false;

        DisplayNode(nextNodeId);
    }

    private void ChangeAffinity(
        string target,
        int delta)
    {
        GameState.Instance.ChangeAffinity(target, delta);
    }

    public int GetAffinity(string target)
    {
        return GameState.Instance.GetAffinity(target);
    }
}
