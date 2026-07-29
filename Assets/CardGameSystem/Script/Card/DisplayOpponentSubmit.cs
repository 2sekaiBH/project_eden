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
    }

    private void DestroyAllCard(int _)
    {
        cards.ForEach((card) => Destroy(card));
        cards.Clear();
    }



}
