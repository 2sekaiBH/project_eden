using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerActor : Actor
{
    [SerializeField] private int energy = 3;
    public int Energy => energy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        name = "Player"; // µð¹ö±ë¿ë
        profileUpdator.UpdateProfile(name, currentHp, currentBlock, energy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
