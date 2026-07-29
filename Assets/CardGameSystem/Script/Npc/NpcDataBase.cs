using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDataBase", menuName = "Scriptable Objects/CardSystem/NpcDataBase")]
public class NpcDataBase : ScriptableObject
{
    [SerializeField] private List<NpcData> _npcDataBase;
    public List<NpcData> npcDataBase => _npcDataBase;
}
