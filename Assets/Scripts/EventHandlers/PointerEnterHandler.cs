using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PointerEnterHandler : MonoBehaviour, IPointerEnterHandler
{
    public Action OnPointerEnterAction;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAction?.Invoke();
    }
}
