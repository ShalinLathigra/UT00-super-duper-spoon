using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Core.Logger;
using Project.Core.ScriptableObjects;
using Project.Core.Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Core.Scene
{
    public interface ISceneLoader : ICoreService
    {

        public UniTask Unload(IEnumerable<SceneField> sceneGroup);
        public UniTask Load(SceneGroup sceneGroup);
    }

    public class SceneLoader : ISceneLoader
    {
        readonly ICoreLogger _logger;

        public SceneLoader(ICoreLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Load in scene and any children. No nesting.
        /// </summary>
        /// <param name="sceneGroup">Contains a set of scenes (not necessarily unique)</param>
        /// <returns></returns>
        public async UniTask Load(SceneGroup sceneGroup)
        {
            AsyncOperation[] asyncOperations = sceneGroup.childScenes.Where(s => !s.Loaded)
                .Select(s => SceneManager.LoadSceneAsync(s.SceneName, LoadSceneMode.Additive)).ToArray();
            await UniTask.WaitUntil(() => asyncOperations.Sum(s => s.isDone ? 0 : 1) <= 0);
        }

        /// <summary>
        /// Will unload exactly one scene. to do multiple, must call it once for each.
        /// </summary>
        public async UniTask Unload(IEnumerable<SceneField> oldScenes)
        {
            AsyncOperation[] asyncOperations = oldScenes.Where(s => s.Loaded)
                .Select(s => SceneManager.UnloadSceneAsync(s.SceneName)).ToArray();
            await UniTask.WaitUntil(() => asyncOperations.Sum(s => s.isDone ? 0 : 1) <= 0);
        }
    }
}