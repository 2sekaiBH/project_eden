using System;

[Serializable]
public class DialogueFile
{
    public string startId;
    public DialogueNode[] nodes;
}

[Serializable]
public class DialogueNode
{
    public string id;
    public string type;

    public string background;
    public float dimAlpha;
    public bool fadeDim;
    public float fadeDuration;

    public string speaker;
    public string text;

    public string leftCharacter;
    public string rightCharacter;

    public string sfx;
    public string nextId;

    // 선택지 노드일 때만 사용
    public ChoiceData[] choices;
}

[Serializable]
public class ChoiceData
{
    public string text;
    public string nextId;

    // architect / cain / noah
    public string affinityTarget;

    // 증가 1, 유지 0, 감소 -1
    public int affinityDelta;
}