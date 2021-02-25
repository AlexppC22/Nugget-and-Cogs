using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CogInvetorySlot : MonoBehaviour, IDropHandler
{
    [Header("> Referências")]
    [SerializeField,Tooltip("Proprio RectTransform")] private RectTransform _rectTransform;
    private void Start()
    {
        if (this._rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = _rectTransform.anchoredPosition;
        eventData.pointerDrag.GetComponent<CogController>().SetState(CogState.inInventory);
    }
}
