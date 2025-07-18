using UnityEngine;
using UnityEngine.EventSystems;

public class AreaDeMontagem : MonoBehaviour, IDropHandler
{
    public GerenciadorDePilha gerenciadorDePilha; // Referência ao script GerenciadorDePilha

    public void OnDrop(PointerEventData eventData)
{
    GameObject bloco = eventData.pointerDrag;

    if (bloco != null)
    {
        bloco.transform.SetParent(transform);
        bloco.transform.localScale = Vector3.one;

        // Garante que o bloco resete a posição relativa ao layout
        bloco.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // ✅ Chama o processamento do comando
        ProcessarComando(bloco.name);
    }
}


    private void ProcessarComando(string comando)
    {
        if (comando == "CriarPilha")
        {
            gerenciadorDePilha.CriarPilha();
        }
        else if (comando == "AlocarMemoria")
        {
            gerenciadorDePilha.AlocarMemoria();
        }
        else if (comando == "PushSolar")
        {
            gerenciadorDePilha.EmpilharBateria("Solar");
        }
        else if (comando == "PushEletrica")
        {
            gerenciadorDePilha.EmpilharBateria("Elétrica");
        }
        else
        {
            Debug.Log("Comando não reconhecido.");
        }
    }
}
