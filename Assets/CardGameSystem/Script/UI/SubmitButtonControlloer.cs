using System;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButtonControlloer : MonoBehaviour
{
    [SerializeField] private Button submitButton;
    private Action OnPlayerStartSelectHandler;

    private void Awake()
    {
        if(submitButton == null)
            submitButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        OnPlayerStartSelectHandler = () =>  SetInteractiveBtn(true);
        PlayerActor.OnPlayerStartSelect += OnPlayerStartSelectHandler;
    }

    private void OnDisable()
    {
        PlayerActor.OnPlayerStartSelect -= OnPlayerStartSelectHandler;
    }

    private void SetInteractiveBtn(bool active)
    {
        submitButton.interactable = active;
    }

}
