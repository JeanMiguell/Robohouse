using UnityEngine;
using UnityEngine.EventSystems;

public class Arrastavel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // Referência ao script do compilador
    
    // Posição inicial do botão, caso ele precise voltar para o fundo de código
    private Vector3 posInicial;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        // Guarda a posição inicial do botão
        posInicial = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Reduz a opacidade durante o arrasto
        canvasGroup.blocksRaycasts = false; // Permite passar pelos raycasts enquanto arrasta
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Move o objeto
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // Restaura a opacidade
        canvasGroup.blocksRaycasts = true; // Bloqueia os raycasts novamente

        // Verifica se o botão foi solto na área do painel de compilador
    }
}
