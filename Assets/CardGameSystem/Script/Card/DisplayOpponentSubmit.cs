using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOpponentSubmit : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform rectTransform;

    private List<GameObject> cards = new List<GameObject>();

    private void OnEnable()
    {
        OpponentActor.OnOpponentEndSelect += DisplayOpponentsCard;
        TurnFlowManager.OnTurnEnd += DestroyAllCard;
    }

    private void OnDisable()
    {
        OpponentActor.OnOpponentEndSelect -= DisplayOpponentsCard;
        TurnFlowManager.OnTurnEnd -= DestroyAllCard;
    }
    private void Awake()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
    }

    public void DisplayOpponentsCard(List<CardData> selectedCardData)
    {
        foreach (CardData cardData in selectedCardData)
        {
            GameObject newCard = Instantiate(cardPrefab, rectTransform, false);
            cards.Add(newCard);
            newCard.GetComponent<CardDisplay>().SetCard(cardData);
        }

        StartCoroutine(FlipAllOpponentCards());
    }

    private void DestroyAllCard(int _)
    {
        cards.ForEach((card) => Destroy(card));
        cards.Clear();
    }

    //카드 촤라락
    private IEnumerator FlipAllOpponentCards()
    {
        yield return null; // 안정화 (이거 중요)

        for (int i = 0; i < cards.Count; i++)
        {
            CardDisplay display = cards[i].GetComponent<CardDisplay>();
            StartCoroutine(display.Flip());

            yield return new WaitForSeconds(0.1f); //연출용
        }
    }



}
