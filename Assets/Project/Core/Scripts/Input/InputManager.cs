using System;
using Project.Core.Service;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Core.Input
{
    /// <summary>
    /// Wraps around a player input
    /// </summary>
    public interface IInputManager : ICoreService
    {
        public InputAction GetAction(string actionName);
        public IObservable<InputAction.CallbackContext> GetObservable(string actionName);
    }
    public class InputManager : MonoBehaviour, IInputManager
    {
        [SerializeField] PlayerInput input;

        void Awake()
        {
            ServiceLocator.Instance.TryRegister(this as IInputManager);
        }

        public InputAction GetAction(string actionName)
        {
            return input.currentActionMap.FindAction(actionName, true);
        }

        public IObservable<InputAction.CallbackContext> GetObservable(string actionName)
        {
            return GetAction(actionName).AsObservable();
        }
    }

    public static class InputExtensions
    {
        
        public static IObservable<InputAction.CallbackContext> AsObservable(this InputAction action) =>
            Observable.FromEvent<InputAction.CallbackContext>(
                h =>
                {
                    action.started += h;
                    action.performed += h;
                    action.canceled += h;
                }, 
                h =>
                {
                    action.started -= h;
                    action.performed -= h;
                    action.canceled -= h;
                });
    }
    
    
    // Basically, I want the input manager to be the gobetween between player buttons and the resulting functions.
    // Nevermind, what should actually happen is, the input manager stores a few things.
    /*  1. The current set of inputs. i.e. Left Click, right click, vector2 movement
     *  2. Any input blockers that are active
     *  Things that need player input will use InputManager.GetSomething to retrieve the current value of that input.
     *
     * Subscribing to the actual events is a bit more awkward, and Honestly, fuck that. Use
     * uniRx to determine the time an input is set to 0 and the action is not on cooldown.
     */
    // 
}