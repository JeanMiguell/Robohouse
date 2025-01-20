using UnityEngine;
using UnityEngine.EventSystems;

public class AreaDeMontagem : MonoBehaviour, IDropHandler
{
    public GerenciadorDePilha gerenciadorDePilha; // Referência ao script GerenciadorDePilha

    public void OnDrop(PointerEventData eventData)
    {
        GameObject bloco = eventData.pointerDrag; // O objeto que foi arrastado
        if (bloco != null)
        {
            Debug.Log($"Comando Solto: {bloco.name}");
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
        else if (comando == "PushEle    trica")
        {
            gerenciadorDePilha.EmpilharBateria("Elétrica");
        }
        else
        {
            Debug.Log("Comando não reconhecido.");
        }
    }
}
