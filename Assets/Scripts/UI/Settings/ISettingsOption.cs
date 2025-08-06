using System;
using Assets.Scripts.CustomEventArgs;

namespace Assets.Scripts.UI.Settings
{
    public interface ISettingsOption<T>
    {
        public event EventHandler<ValueEventArgs<T>> OnValueChanged;
    }
}