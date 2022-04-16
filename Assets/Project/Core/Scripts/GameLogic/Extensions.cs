using System;
using UniRx;
using UnityEngine.InputSystem;

namespace Project.Core.GameLogic
{
    public static class Extensions
    {
        public static IObservable<InputAction.CallbackContext> AsObservable(this InputAction action) =>
            Observable.FromEvent<InputAction.CallbackContext>(
                h =>
                {
                    action.performed += h;
                    action.canceled += h;
                },
                h =>
                {
                    action.performed -= h;
                    action.canceled -= h;
                });
    }
}
