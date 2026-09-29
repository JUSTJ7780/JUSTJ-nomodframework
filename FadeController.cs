using UnityEngine;

namespace NOModFramework
{
    public sealed class FadeController
    {
        private bool _targetOpen;
        private float _duration = 0.14f;

        public float Alpha { get; private set; }

        public void SetOpen(bool open, bool instant)
        {
            _targetOpen = open;

            if (instant)
            {
                Alpha = open ? 1f : 0f;
            }
        }


        public void Tick(float deltaTime)
        {
            float target = _targetOpen ? 1f : 0f;

            if (Mathf.Approximately(Alpha, target))
            {
                Alpha = target;
                return;
            }

            if (_duration <= 0f)
            {
                Alpha = target;
                return;
            }

            Alpha = Mathf.MoveTowards(Alpha, target, deltaTime / _duration);
        }
    }
}