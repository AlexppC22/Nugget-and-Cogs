using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CogState
{
    inInventory,
    inWorld,
    inUpperWorldSlot,
    inLowerWorldSlot,
    restart
}

public class CogController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    #region Vars
    [Header("> Referências")]
    [SerializeField, Tooltip("Canvas principal do jogo")] Canvas mainCanvas;
    [SerializeField, Tooltip("Rect Transform da engrenagem")] RectTransform _rectTransform;
    [SerializeField, Tooltip("Referência visual do estado")] CogState cogState;

    [Header("> Sprite")]
    [Tooltip("Imagem da engrenagem")] public Image _image;
    [Tooltip("Sprites que ela pode se tornar")] public Sprite[] cogSprites;

    [Header("> Posições")]
    Vector2 startPos;
    
    [Header("> Escalas")]
    Vector2 NormalScale = new Vector2(1,1);
    Vector2 HoverScale = new Vector2(1.2f,1.2f);
    [SerializeField, Tooltip("Escala normal do 1° sprite")] Vector2 FirstNormalScale;
    [SerializeField, Tooltip("Escala de quando o mouse é posto acima do 1° sprite")] Vector2 FirstHoverScale;
    [SerializeField, Tooltip("Escala normal do 2° sprite")] Vector2 SecondNormalScale;
    [SerializeField, Tooltip("Escala de quando o mouse é posto acima do 2° sprite")] Vector2 SecondHoverScale;

    bool wasInCorrectPlace;
    bool isUp;
    #endregion
    #region Unity Callbacks
    private void Start()
    {
        //Define a posição pra qual a engrenagem volta quando o jogo é reiniciado
        startPos = this._rectTransform.position;
        //Escalas iniciais pra Feedback visual
        NormalScale = FirstNormalScale;
        HoverScale = FirstHoverScale;
    }
    private void Update()
    {
        if(GameManager.instance.canSpin)
        {
            if(isUp)
                this._rectTransform.Rotate(0, 0, -1);
            else
                this._rectTransform.Rotate(0, 0, 1);
        }
    }
    #endregion
    #region StateMachine
    /// Serve pra troca de sprite e verificação de pontos
    public void SetState(CogState state)
    {
        switch(state)
        {
            case CogState.inInventory:
                this._image.sprite = cogSprites[0];
                break;

            case CogState.inWorld:
                if(wasInCorrectPlace)
                {
                    GameManager.instance.cogsInPlace--;
                    GameManager.instance.CheckWin();
                    wasInCorrectPlace = false;
                }
                this._image.sprite = cogSprites[0];
                break;

            case CogState.inUpperWorldSlot:
                isUp = true;
                this._image.sprite = cogSprites[1];
                GameManager.instance.cogsInPlace++;
                GameManager.instance.CheckWin();
                wasInCorrectPlace = true;
                break;

            case CogState.inLowerWorldSlot:
                isUp = false;
                this._image.sprite = cogSprites[1];
                GameManager.instance.cogsInPlace++;
                GameManager.instance.CheckWin();
                wasInCorrectPlace = true;
                break;

            case CogState.restart:
                this._rectTransform.position = startPos;
                if (wasInCorrectPlace)
                    wasInCorrectPlace = false;
                SetState(CogState.inInventory);
                break;
        }
        #region ChageCogSize
        if (this._image.sprite == cogSprites[0])
        {
            NormalScale = FirstNormalScale;
            HoverScale = FirstHoverScale;
        }
        else
        {
            NormalScale = SecondNormalScale;
            HoverScale = SecondHoverScale;
        }
        #endregion
        cogState = state;
    }
    #endregion
    #region Drag
    //Chamado todo Frame enquanto é arrastado (Sofre "Drag)
    public void OnDrag(PointerEventData eventData)
    {
        //Para poder seguir a posição correta do mouse
        _rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }
    //Chamado no inicio do "Drag" (Arrasto)
    public void OnBeginDrag(PointerEventData eventData)
    {
        SetState(CogState.inWorld);
        _image.raycastTarget = false;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0.5f);
    }
    //Chamado no fim do "Drag" (Arrasto)
    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
    }
    #endregion
    #region Hover
    public void OnHoverEnter()
    {
        this._rectTransform.localScale = HoverScale;
    }
    public void OnHoverExit()
    {
        this._rectTransform.localScale = NormalScale;
    }
    #endregion 
}
