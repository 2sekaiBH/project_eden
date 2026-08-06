using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class PlayerInputKeySettingController : KeySettingController
{
    [SerializeField] private string inputActionName;
    [SerializeField] private string[] inputMapName;
    private PlayerInput playerInput;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        if(KeyManager.Instance == null)
        {
            Debug.LogWarning("KeyManager가 없습니다.");
            return;
        }
        // playerInput =  KeyManager.Instance.PlayerInput;
    }

    private void RebindAction()
    {
        // 기준이 될 첫 번째 Map의 Action만 Interactive Rebinding
        InputActionMap map = playerInput.actions.FindActionMap(inputMapName[0]);
        InputAction action = map.FindAction(inputActionName);

        if (action == null)
        {
            Debug.LogWarning("해당 Action을 찾을 수 없습니다.");
            return;
        }

        // 기존 리바인딩이 진행 중이면 종료
        rebindingOperation?.Dispose();

        // Action 비활성화
        action.Disable();

        rebindingOperation = action.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>")
            .OnComplete(operation =>
            {
                action.Enable();

                string newPath = action.bindings[0].effectivePath;

                Debug.Log($"새 바인딩 : {action.bindings[0].effectivePath}");

                ApplyBindingToAllMaps(newPath);

                operation.Dispose();
                rebindingOperation = null;
            })
            .Start();
    }
    private void ApplyBindingToAllMaps(string newPath)
    {
        foreach (string mapName in inputMapName)
        {
            InputActionMap map = playerInput.actions.FindActionMap(mapName);

            if (map == null)
                continue;

            InputAction action = map.FindAction(inputActionName);

            if (action == null)
                continue;

            action.ApplyBindingOverride(0, newPath);
        }
    }
}

