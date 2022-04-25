using Project.Core.Input;
using Project.Core.Logger;
using Project.Core.Service;
using UniRx;
using UnityEngine;

namespace Project.Core.GameLogic
{
    public class Player : MonoBehaviour
    {
        [SerializeField] CoreLoggerMono logger;
        IInputManager _inputManager;
         Vector2 _direction;

         /// <summary>
         /// USE START METHOD TO REGISTER ALL OF OUR INPUT METHODS!!!!
         /// </summary>
        void Start()
        {
            if (!ServiceLocator.Instance.TryGet(out _inputManager))
                logger.Fatal("No Input Manager Present");

            // I AM SO GODDAMN BIG BRAIN
            // I absolutely love code.
            _inputManager.GetObservable<Vector2>("Move", ActionEnum.Performed | ActionEnum.Canceled)
                .Subscribe(newDir => _direction = newDir).AddTo(this);
        }
    }
}