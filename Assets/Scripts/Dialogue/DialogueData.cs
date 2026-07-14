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
}
