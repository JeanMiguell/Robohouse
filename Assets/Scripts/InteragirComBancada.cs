using UnityEngine;
using System.Linq;


public class InteragirComBancada : MonoBehaviour
{
    public GameObject prefabPedido;  // Prefab do pedido a ser criado
    private GameObject pedidoAtual;  // Guarda a referência do pedido instanciado
    private bool pertoDaCabine = false;  // Verifica se o jogador está perto da cabine
    public GerenciadorDePilha gerenciadorDePilha;
    public GameObject pedidoVisual; // O pedido visual na tela principal
    public PedidosDaFase pedidosDaFase; // Referência ao ScriptableObject
    private int proximoPedidoIndex = 0; // Controle interno

    void Update()
    {
        // Verifica se o jogador está perto da cabine e pressionou "E"
        if (pertoDaCabine && Input.GetKeyDown(KeyCode.E))
        {
            // Verifica se já há um pedido ativo
            if (pedidoAtual == null)
            {
                CriarPedido();
            }
        }
    }

    // Detecta quando o jogador entra ou sai da área da cabine
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Certifique-se de que o jogador tem a tag "Player"
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

    // Função para criar o pedido
    void CriarPedido()
{
    if (proximoPedidoIndex >= pedidosDaFase.pedidosDisponiveis.Length)
    {
        Debug.Log("Todos os pedidos foram coletados.");
        return;
    }

    // Instancia o visual do pedido na posição definida
    pedidoAtual = Instantiate(prefabPedido, new Vector3(3.32f, 3.36f, 0), Quaternion.identity);

    // Pega o componente Pedido
    Pedido pedido = pedidoAtual.GetComponent<Pedido>();
    
    if (pedido != null)
    {
        // Pega os dados do ScriptableObject
        TipoBateria[] bateriasDoPedido = pedidosDaFase.pedidosDisponiveis[proximoPedidoIndex].baterias;

        if (bateriasDoPedido == null || bateriasDoPedido.Length == 0)
        {
            Debug.LogError("O pedido configurado no ScriptableObject está vazio!");
            return;
        }

        // Define as baterias no visual e avança pro próximo
        pedido.tiposDeBaterias = bateriasDoPedido;
        proximoPedidoIndex++;

        // Conecta com o Gerenciador de Pilha
        if (gerenciadorDePilha != null)
        {
            gerenciadorDePilha.pedidoVisual = pedidoAtual;
            gerenciadorDePilha.ConfigurarPedido(bateriasDoPedido.Select(b => b.ToString()).ToList());
        }
        else
        {
            Debug.LogError("GerenciadorDePilha não está atribuído no Inspector.");
        }
    }
    else
    {
        Debug.LogError("O prefab do pedido não possui o script Pedido.cs.");
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
}

}
