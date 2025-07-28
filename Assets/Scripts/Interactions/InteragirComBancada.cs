using UnityEngine;
using System.Linq;
using RoboHouse.Data;
using RoboHouse.Gameplay;

public class InteragirComBancada : MonoBehaviour
{
    public GameObject prefabPedido;
    private GameObject pedidoAtual;
    private bool pertoDaCabine = false;

    public StructureExecutor structureExecutor;
    public PedidosDaFase pedidosDaFase;
    private int proximoPedidoIndex = 0;

    void Update()
    {
        if (pertoDaCabine && Input.GetKeyDown(KeyCode.E))
        {
            if (pedidoAtual == null)
            {
                CriarPedido();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pertoDaCabine = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pertoDaCabine = false;
        }
    }

    void CriarPedido()
    {
        if (proximoPedidoIndex >= pedidosDaFase.pedidosDisponiveis.Length)
        {
            Debug.Log("Todos os pedidos foram coletados.");
            return;
        }

        pedidoAtual = Instantiate(prefabPedido, new Vector3(3.32f, 3.36f, 0), Quaternion.identity);

        PedidoVisual pedido = pedidoAtual.GetComponent<PedidoVisual>();

        if (pedido != null)
        {
            TipoBateria[] bateriasDoPedido = pedidosDaFase.pedidosDisponiveis[proximoPedidoIndex].baterias;

            if (bateriasDoPedido == null || bateriasDoPedido.Length == 0)
            {
                Debug.LogError("O pedido configurado no ScriptableObject está vazio!");
                return;
            }

            pedido.tiposDeBaterias = bateriasDoPedido;
            proximoPedidoIndex++;

            if (structureExecutor != null)
            {
                structureExecutor.PedidoVisual = pedidoAtual;
                structureExecutor.ConfigurarPedido(bateriasDoPedido.Select(b => b.ToString()).ToList());
            }
            else
            {
                Debug.LogError("StructureExecutor não está atribuído no Inspector.");
            }
        }
        else
        {
            Debug.LogError("O prefab do pedido não possui o script PedidoVisual.cs.");
        }
    }

    public void PedidoEntregue()
    {
        if (pedidoAtual != null)
        {
            Destroy(pedidoAtual);
            pedidoAtual = null;
        }

        Debug.Log("Pedido entregue e liberado para novo pedido.");

        // Resetar blocos de programação (opcional)
        FindObjectOfType<ComandoResetController>()?.ResetarTodosBlocos();
    }
}
