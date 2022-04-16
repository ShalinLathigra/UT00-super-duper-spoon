using UnityEngine;

namespace Project.Core.GameLogic
{
    public class Balloon : MonoBehaviour
    {
        /// <summary>
        /// Basically, this affects the rate of increase in size.
        /// Initial inflate is big, initial deflate is small
        /// Both will even out after a bit.
        /// </summary>
        public AnimationCurve inflateRateCurve;
        public AnimationCurve deflateRateCurve;

        bool _pumping;
        float _scaleMod = 1f;
        float _t;

        /// <summary>
        /// Used by the minigame controller to pass the input data along.
        /// </summary>
        /// <param name="state"></param>
        public void ToggleDirection(bool state)
        {
            _t = 0;
            _pumping = state;
        }

        void Update()
        {
            _t += Time.deltaTime;
            _scaleMod = _pumping
                ? _scaleMod + inflateRateCurve.Evaluate(_t) * Time.deltaTime
                : Mathf.Max(1f, _scaleMod - deflateRateCurve.Evaluate(_t) * Time.deltaTime);
            transform.localScale = Vector3.one * _scaleMod;
        }
    }
}
