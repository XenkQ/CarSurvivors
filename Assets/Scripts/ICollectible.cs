using Assets.Scripts.Providers;
using System;

namespace Assets.Scripts
{
    public interface ICollectible : IGameObjectProvider
    {
        public event EventHandler OnCollected;
    }
}
