
using System.Collections.Generic;
using UnityEngine;

public class NpcEffectExecutor : MonoBehaviour
{
    private void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void NpcEffectExecute(List<NpcEffect> npcEffectsList)
    {
        npcEffectsList.ForEach((effect) => effect.Apply());
    }
}
