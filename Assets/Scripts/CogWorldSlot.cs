using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CogWorldSlot : MonoBehaviour, IDropHandler
{
    [SerializeField, Tooltip("Proprio RectTransform")] private RectTransform _rectTransform;
    [SerializeField, Tooltip("Define se está na posição de cima ou embaixo")] bool isUpper;

    private void Start()
    {
        if(this._rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = _rectTransform.anchoredPosition;
        if (isUpper)
        {
            eventData.pointerDrag.GetComponent<CogController>().SetState(CogState.inUpperWorldSlot);
        }
        else
            eventData.pointerDrag.GetComponent<CogController>().SetState(CogState.inLowerWorldSlot);
    }

}
