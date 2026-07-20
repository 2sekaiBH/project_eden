using UnityEngine;

[CreateAssetMenu(fileName = "OpponentData", menuName = "Scriptable Objects/CardSystem/OpponentData")]
public class OpponentData : ScriptableObject
{
    public new string name;
    public int totalHp;
    public int maxEnergy = 4;
}
