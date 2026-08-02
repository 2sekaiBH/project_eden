using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 덱을 관리하는 스크립트
/// </summary>
public class DeckManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CardDataBase CardDataBase; // 카드 데이터 베이스
    
    public List<CardData> deck; // 카드 덱

    private static DeckManager instance; // 싱글톤
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
    /// 랜덤 카드 뽑기 함수
    /// </summary>
    /// <param name="amount">뽑을 카드 갯수</param>
    /// <returns>뽑은 카드</returns>
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
    /// 기본 덱(10장)을 초과하는 extraCard 뽑기 함수
    /// deck을 거치지 않고 cardDataBase에서 직접 pick
    /// </summary>
    /// <param name="amount">뽑을 카드 갯수</param>
    /// <returns>뽑은 카드</returns>
    public List<CardData> DrawExtrCard(int amount)
    {
        List<CardData> pickedCardDeck = new List<CardData>();
        int random;
        for (int i = 0; i < amount; i++)
        {
            random = Random.Range(0, CardDataBase.cardDataBase.Count);
            pickedCardDeck.Add(Instantiate(CardDataBase.cardDataBase[random])); //extra로 뽑는 카드는 복사본으로 생성
        }
        return pickedCardDeck;
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void InitializeDeck()
    {
        deck.Clear();
        deck.AddRange(CardDataBase.cardDataBase);
    }
}
