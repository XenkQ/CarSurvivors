using System;

namespace Assets.Scripts
{
    public interface ICollectible
    {
        public event EventHandler OnCollected;
    }
}
