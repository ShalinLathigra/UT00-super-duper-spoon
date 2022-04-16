using Cysharp.Threading.Tasks;
using Project.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace Project.Core.Scene
{
    /// <summary>
    /// Optional sceneController, handles playing transitions if assigned.
    /// Can define an intro, an outro, or none.
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public class SceneController : MonoBehaviour
    {
        [SerializeField] AnchorSceneGroup origin;
        public PlayableDirector director;
        public PlayableAsset intro;
        public PlayableAsset outro;
        bool _isIntroNotNull;
        bool _isOutroNotNull;

        void Start()
        {
            _isIntroNotNull = intro != null;
            _isOutroNotNull = intro != null;
            origin.Controller = this;
        }

        public async UniTask PlayIntro()
        {
            if (!_isIntroNotNull) return;
            director.Play(intro);
            while (director.state != PlayState.Playing)
            {
                await UniTask.Yield();
            }
        }

        public async UniTask PlayOutro()
        {
            if (!_isOutroNotNull) return;
            director.Play(outro);
            while (director.state != PlayState.Playing)
            {
                await UniTask.Yield();
            }
        }
    }
}
