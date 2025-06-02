using System;

namespace Assets.Scripts.UI
{
    public struct ClickableButtonData
    {
        public string Text { get; set; }
        public Action OnClick { get; set; }
    }
}
