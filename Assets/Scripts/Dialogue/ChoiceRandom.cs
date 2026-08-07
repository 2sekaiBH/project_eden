using System.Net.Sockets;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private void OnEnable()
    {
        Shuffle();
    }

    void Shuffle()
    {

        int count = transform.childCount; //선택지 개수

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(i, count);

            Transform a = transform.GetChild(i);
            Transform b = transform.GetChild(rand);

            int indexA = a.GetSiblingIndex();
            int indexB = b.GetSiblingIndex();

            a.SetSiblingIndex(indexB);
            b.SetSiblingIndex(indexA);
        }
    }
}
