using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CardDataBase CardDataBase;
    
    public List<CardData> deck; // Ä«µå µ¦

    private static DeckManager instance;
    public static DeckManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        InitializeDeck();
    }
    void Start()
    {
        
    }

    // ·£´ý Ä«µå »Ì±â ÇÔ¼ö(»ÌÀ» Ä«µå °¹¼ö)
    public List<CardData> DrawRandomCard(int amount)
    {
        List<CardData> pickedCardDeck = new List<CardData>();
        int random;
        for(int i = 0; i < amount; i++)
        {
            random = Random.Range(0, deck.Count);
            pickedCardDeck.Add(deck[random]);
            deck.RemoveAt(random);
        }
        return pickedCardDeck;
    }

    public void InitializeDeck()
    {
        deck.Clear();
        deck.AddRange(CardDataBase.cardDataBase);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
