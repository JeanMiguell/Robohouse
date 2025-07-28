using UnityEngine;
using UnityEngine.EventSystems;
using RoboHouse.Gameplay;
using RoboHouse.Data;

public class AreaDeMontagem : MonoBehaviour, IDropHandler
{

    public StructureExecutor structureExecutor; // no topo da classe

    GLBoard gLBoard;
  
    public void OnDrop(PointerEventData eventData)
    {
        GameObject bloco = eventData.pointerDrag;

        if (bloco != null)
        {
            Debug.Log("Comando recebido: " + bloco.name);
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
            structureExecutor.CriarPilha();
        }
        else if (comando == "AlocarMemoria")
        {
            structureExecutor.AlocarMemoria();
        }   
        else if (comando == "PushSolar")
        {
            structureExecutor.EmpilharBateria(TipoBateria.Solar);
        }
        else if (comando == "PushEletrica")
        {
            structureExecutor.EmpilharBateria(TipoBateria.Eletrica);
        }
        else
        {
            Debug.Log("Comando não reconhecido.");
        }
    }
}
