using UnityEngine;

[CreateAssetMenu(fileName = "NpcData", menuName = "Scriptable Objects/CardSystem/NpcData")]
public class NpcData : ScriptableObject
{
    public new string name;
    public string effectDescription;
    public Sprite npcProfileImage;
}
