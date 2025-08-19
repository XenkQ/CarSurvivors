using System;

namespace Assets.Scripts
{
    public interface IPoolable
    {
        public void OnGet();

        public void OnRelease();

        public void ReturnToPool();

        public event EventHandler OnCanBeReleased;
    }
}
