using Cysharp.Threading.Tasks;
using Project.Core.Scene;
using UnityEngine;

namespace Project.Core.ScriptableObjects
{
    /// <summary>
    /// Pretty much a pure data container with information about a scene, root of this all is stored in the SceneLib
    /// When an "anchor" scene is loaded
    /// </summary>
    [CreateAssetMenu(menuName = "SceneLoader/Create AnchorSceneData", fileName = "AnchorSceneData", order = 1)]
    public class AnchorSceneGroup : SceneGroup
    {
        TransitionController _controller;
        bool _controllerFound;
        
        /// <summary>
        /// Lazily get the sceneController. Every scene has to have one
        /// </summary>
        public TransitionController TransitionController
        {
            get => _controllerFound ? _controller : null;
            set
            {
                _controllerFound = value != null;
                if (_controllerFound) _controller = value;
            }
        }

        public async UniTask PlayOutro()
        {
            if (_controllerFound) await TransitionController.PlayOutro();
        }

        public async UniTask PlayIntro()
        {
            if (_controllerFound) await TransitionController.PlayIntro();
        }
    }
}