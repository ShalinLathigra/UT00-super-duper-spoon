using System;
using Project.Core.Input;
using Project.Core.Logger;
using Project.Core.Service;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Core.GameLogic
{
    public class Player : MonoBehaviour
    {
        /* So what is this actually responsible for?
     * I like thinking of it as Brain
     *
     * Use c# events to determine when buttons are pressed or released.
     * With this information, we can here know immediately what is going on.
     */
        [SerializeField] CoreLoggerMono logger;
        IInputManager _inputManager;
        Vector2 _direction;

        void Start()
        {
            if (!ServiceLocator.Instance.TryGet(out _inputManager))
                logger.Fatal("No Input Manager Present");

            //InputAction moveAction = _inputManager.GetAction("Move");

            //todo: should extend this method to allow variable shit. GetObservable<T> kinda dealio stuff. Let you specify arg type as well as what conditions to fire on.
            _inputManager.GetObservable("Move").Where(action => action.performed || action.canceled)
                .Select(action =>
                {
                    Debug.Log(action.phase.ToString());
                    return action.ReadValue<Vector2>();
                }).Subscribe(LogOutput).AddTo(this);
        }

        void LogOutput(Vector2 input)
        {
            logger.Info(input);
        }
    }
}