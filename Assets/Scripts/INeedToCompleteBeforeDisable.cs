using System;

namespace Assets.Scripts
{
    public interface INeedToCompleteBeforeDisable
    {
        public event EventHandler OnCompleted;
    }
}
