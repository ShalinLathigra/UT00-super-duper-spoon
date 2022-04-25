using System;
using Project.Core.Service;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Core.Input
{
    [Flags]
    public enum ActionEnum
    {
        Started=1,
        Performed=2,
        Canceled=4,
        All=7,
    }
    /// <summary>
    /// Wraps around a player input
    /// </summary>
    public interface IInputManager : ICoreService
    {
        public InputAction GetAction(string actionName);
        public IObservable<InputAction.CallbackContext> GetObservable(string actionName, ActionEnum targetFlags);
        public IObservable<T> GetObservable<T>(string actionName, ActionEnum targetFlags) where T : struct;
    }
    public class InputManager : MonoBehaviour, IInputManager
    {
        [SerializeField] PlayerInput input;

        void Awake() => 
            ServiceLocator.Instance.TryRegister(this as IInputManager);

        /// <summary>
        /// Exposes access to individual Unity InputActions that are defined.
        /// </summary>
        /// <param name="actionName">String name of action to target</param>
        /// <returns>Unity Input Action</returns>
        public InputAction GetAction(string actionName) => 
            input.currentActionMap.FindAction(actionName, true);
        
        /// <summary>
        /// Returns an IObservable<InputAction.CallbackContext
        /// </summary>
        /// <param name="actionName"> Name of InputAction to listen for</param>
        /// <param name="targetFlags"> ActionEnum | other Action Enum. Specifies what action triggers to use.</param>
        /// <returns></returns>
        public IObservable<InputAction.CallbackContext> GetObservable(string actionName, ActionEnum targetFlags)
        {
            return GetAction(actionName)
                .AsObservable()
                .Where(action =>
                {
                    ActionEnum actionFlags = (action.started ? ActionEnum.Started : 0) |
                                             (action.performed ? ActionEnum.Performed : 0) |
                                             (action.canceled ? ActionEnum.Canceled : 0);
                    return (targetFlags & actionFlags) != 0;
                });
        }
        public IObservable<T> GetObservable<T>(string actionName, ActionEnum targetFlags) where T : struct
        {
            // Alrighty, so how exactly are we going to go from our event flags to 
            // where given an action, AC
            return GetObservable(actionName, targetFlags).Select(action => action.ReadValue<T>());
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