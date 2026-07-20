using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CardDataBase CardDataBase; // Ä«µå µ¥ÀÌÅÍ º£ÀÌ½º
    
    public List<CardData> deck; // Ä«µå µ¦

    private static DeckManager instance; // ½Ì±ÛÅæ
    public static DeckManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        InitializeDeck();
    }

    /// <summary>
    /// ·£´ý Ä«µå »Ì±â ÇÔ¼ö
    /// </summary>
    /// <param name="amount">»ÌÀ» Ä«µå °¹¼ö</param>
    /// <returns>»ÌÀº Ä«µå</returns>
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

    /// <summary>
    /// ÃÊ±âÈ­
    /// </summary>
    public void InitializeDeck()
    {
        deck.Clear();
        deck.AddRange(CardDataBase.cardDataBase);
    }
}
