using Assets.Scripts.Providers;
using System;

namespace Assets.Scripts.Collectibles
{
    public interface ICollectible : IGameObjectProvider
    {
        public event EventHandler OnCollected;
    }
}
