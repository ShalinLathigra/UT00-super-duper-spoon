using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Core.Logger;
using Project.Core.ScriptableObjects;
using Project.Core.Service;
using UnityEngine;

namespace Project.Core.Scene
{
    public interface ISceneManager : ICoreService
    {
        public void MoveToScene(AnchorSceneGroup sceneGroup);
    }
    
    
    public class SceneGroupManager : MonoBehaviour, ISceneManager
    {
        public bool forReal = false;
        [SerializeField] AnchorSceneGroup mainMenu;
        [SerializeField] CoreLoggerMono logger;
        ISceneLoader _sceneLoader;

        AnchorSceneGroup _currentScene;

        void Awake()
        {
            ServiceLocator.Instance.TryRegister(this as ISceneManager);
        }

        void Start()
        {
            if (!ServiceLocator.Instance.TryGet(out _sceneLoader))
                logger.Fatal("No SceneLoader Present");
            
            if (forReal)
                MoveToScene(mainMenu);
        }
        
        public async void MoveToScene(AnchorSceneGroup sceneGroup)
        {
            // Load new scene immediately
            UniTask loadObservable = _sceneLoader.Load(sceneGroup);
            UniTask outro = UniTask.CompletedTask;
            // Play outro optionally
            if (_currentScene != default)
            {
                // delay unt
                outro = _currentScene.PlayOutro();
            }
            // Ensure new scene is loaded
            await loadObservable;
            await outro;

            // begin unloading of old scenes
            // first, get scenes that are in old list and not old list
            if (_currentScene != default)
            {
                List<SceneField> newScenes = sceneGroup.childScenes;
                List<SceneField> oldScenes = _currentScene.childScenes;
                IEnumerable<SceneField> targetList = oldScenes.Where(s => !newScenes.Contains(s));
                
                // don't need to track unloading yet
                _sceneLoader.Unload(targetList);
            }

            // update current scene
            _currentScene = sceneGroup;
            await _currentScene.PlayIntro();
        }
    }
}
