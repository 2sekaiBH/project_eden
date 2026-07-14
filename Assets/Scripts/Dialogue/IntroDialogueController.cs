using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [Header("Dialogue UI")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialogueNextHint;
    [SerializeField] private Button dialogueNextButton;

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

    private readonly Dictionary<string, DialogueNode> nodeById = new();
    private readonly Dictionary<string, Sprite> backgroundByKey = new();
    private readonly Dictionary<string, Sprite> characterByKey = new();
    private readonly Dictionary<string, AudioClip> soundByKey = new();

    private DialogueNode currentNode;
    private string playerName = "";
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

        prologueNextButton.onClick.AddListener(Advance);
        dialogueNextButton.onClick.AddListener(Advance);
        nameConfirmButton.onClick.AddListener(ConfirmName);
        nameInputField.onSubmit.AddListener(_ => ConfirmName());
    }

    private void Start()
    {
        DisplayNode(GetStartId());
    }

    private void Update()
    {
        if (isTransitioning || currentNode == null || Keyboard.current == null)
        {
            return;
        }

        if (currentNode.type == "nameInput")
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Advance();
        }
    }

    private void OnDestroy()
    {
        if (prologueNextButton != null)
            prologueNextButton.onClick.RemoveListener(Advance);

        if (dialogueNextButton != null)
            dialogueNextButton.onClick.RemoveListener(Advance);

        if (nameConfirmButton != null)
            nameConfirmButton.onClick.RemoveListener(ConfirmName);

        if (nameInputField != null)
            nameInputField.onSubmit.RemoveAllListeners();
    }

    private bool ValidateRequiredReferences()
    {
        bool valid =
            dialogueJson != null &&
            prologueScreen != null &&
            nameInputScreen != null &&
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
            dialogueNextButton != null;

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
        SetOnlyScreen(prologueScreen);
        SetDimVisibility(usePrologueDim: true);

        prologueText.text = "";
        prologueNextIndicator.SetActive(false);
        prologueNextButton.interactable = false;

        yield return SetDimAlpha(
            prologueDim,
            node.dimAlpha,
            node.fadeDim,
            node.fadeDuration
        );

        prologueText.text = ReplacePlayerName(node.text);
        prologueNextIndicator.SetActive(true);
        prologueNextButton.interactable = true;
    }

    private void ShowNameInputNode(DialogueNode node)
    {
        SetOnlyScreen(nameInputScreen);
        SetDimVisibility(usePrologueDim: false, hideBoth: true);

        namePromptText.text = ReplacePlayerName(node.text);
        nameInputField.text = "";
        nameInputField.interactable = true;
        nameConfirmButton.interactable = true;

        if (nameErrorText != null)
        {
            nameErrorText.text = "";
        }

        nameInputField.ActivateInputField();
    }

    private IEnumerator ShowDialogueNode(DialogueNode node)
    {
        SetOnlyScreen(characterDialogueScreen);
        SetDimVisibility(usePrologueDim: false);

        speakerNameText.text = ReplacePlayerName(node.speaker);
        speakerNameText.gameObject.SetActive(
            !string.IsNullOrWhiteSpace(speakerNameText.text)
        );

        dialogueText.text = "";
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

        dialogueText.text = ReplacePlayerName(node.text);
        dialogueNextHint.SetActive(true);
        dialogueNextButton.interactable = true;
    }

    public void Advance()
    {
        if (isTransitioning ||
            currentNode == null ||
            currentNode.type == "nameInput")
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

        string enteredName = nameInputField.text.Trim();

        if (string.IsNullOrWhiteSpace(enteredName))
        {
            if (nameErrorText != null)
            {
                nameErrorText.text = "이름을 한 글자 이상 입력해 주세요.";
            }

            nameInputField.ActivateInputField();
            return;
        }

        playerName = enteredName;
        nameInputField.interactable = false;
        nameConfirmButton.interactable = false;

        DisplayNode(currentNode.nextId);
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
        alpha = Mathf.Clamp01(alpha);
        target.gameObject.SetActive(true);
        target.interactable = false;
        target.blocksRaycasts = false;

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
            float t = Mathf.Clamp01(elapsed / duration);
            target.alpha = Mathf.Lerp(0f, alpha, t);
            yield return null;
        }

        target.alpha = alpha;
    }

    private void SetOnlyScreen(GameObject activeScreen)
    {
        prologueScreen.SetActive(activeScreen == prologueScreen);
        nameInputScreen.SetActive(activeScreen == nameInputScreen);
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
        return string.IsNullOrEmpty(value)
            ? ""
            : value.Replace("%%", playerName);
    }

    private void FinishDialogue()
    {
        isTransitioning = false;

        prologueNextButton.interactable = false;
        dialogueNextButton.interactable = false;

        Debug.Log("현재 준비된 인트로 대사가 끝났습니다.");
    }
}
