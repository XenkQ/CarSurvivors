using System;

namespace Assets.Scripts.Enemies
{
    public interface INeedToCompleteBeforeDisable
    {
        public event EventHandler OnCompleted;
    }
}
