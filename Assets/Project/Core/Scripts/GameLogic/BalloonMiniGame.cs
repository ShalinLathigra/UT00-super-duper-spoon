using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Core.GameLogic
{
    [RequireComponent(typeof(PlayerInput))]
    public class BalloonMiniGame : MonoBehaviour
    {
        [SerializeField] PlayerInput input;
        [SerializeField] Balloon balloon;

        InputAction _pumpAction;

        // so what's the basic dealio here?
        // This is responsible for handling updates of all of these objects.
        // Gonna detect player input events, then call the corresponding methods on as needed:
        // Balloon.
        
        // First, need to detect space bar input.

        void Start()
        {
            input.currentActionMap.FindAction("Pump", true).AsObservable()
                .Select(action => action.phase.IsInProgress())
                .Subscribe(balloon.ToggleDirection).AddTo(this);
        }
    }
}