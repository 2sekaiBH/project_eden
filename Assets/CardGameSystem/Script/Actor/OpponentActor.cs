using System.Collections.Generic;
using UnityEngine;

public class OpponentActor : Actor
{
    [SerializeField] private OpponentData opponentData; // name, Hp

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        name = opponentData.name;
        Initialize();

        profileUpdator.UpdateProfile(name, currentHp, currentBlock);
    }
    public override void Initialize()
    {
        currentHp = opponentData.totalHp;
        currentBlock = 0;
    }
}
