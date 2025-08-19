using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Assets.Scripts.EventHandlers
{
public class PointerEnterHandler : MonoBehaviour, IPointerEnterHandler
{
    public Action OnPointerEnterAction;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAction?.Invoke();
    }
}
}