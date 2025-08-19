using System;

namespace Assets.Scripts.ObjectLifeCycle.Actions
{
    public interface INeedToCompleteBeforeDisable
    {
        public event EventHandler OnCompleted;
    }
}
