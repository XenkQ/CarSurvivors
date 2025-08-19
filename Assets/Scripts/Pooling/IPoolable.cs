using System;

namespace Assets.Scripts.Pooling
{
    public interface IPoolable
    {
        public void OnGet();

        public void OnRelease();

        public void ReturnToPool();

        public event EventHandler OnCanBeReleased;
    }
}
