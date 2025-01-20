using UnityEngine;

public class InteragirComBancada : MonoBehaviour
{
    public GameObject prefabPedido;  // Prefab do pedido a ser criado
    private GameObject pedidoAtual;  // Guarda a referência do pedido instanciado
    private bool pertoDaCabine = false;  // Verifica se o jogador está perto da cabine
    public GerenciadorDePilha gerenciadorDePilha;
    public GameObject pedidoVisual; // O pedido visual na tela principal
    private bool pedidoEntregue = true; // Controle para saber se um pedido pode ser coletado
    private bool pertoDaBancada = false; 



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
    if (prefabPedido == null)
    {
        Debug.LogError("PrefabPedido não foi atribuído no Inspector!");
        return;
    }

    if (pedidoAtual != null)
    {
        Debug.LogWarning("Um pedido já foi criado! Não é possível criar outro enquanto o atual não for entregue.");
        return;
    }

    if (!pedidoEntregue) // Garante que o jogador só possa coletar um novo pedido após entregar o anterior
    {
        Debug.LogWarning("Ainda há um pedido ativo! Entregue o pedido atual antes de coletar outro.");
        return;
    }

    // Cria o pedido na posição desejada
    pedidoAtual = Instantiate(prefabPedido, new Vector3(3.32f, 3.36f, 0), Quaternion.identity);

    // Exibe o pedido na tela
    pedidoAtual.SetActive(true);

    // Configura o pedido como visual no Gerenciador de Pilha
    if (gerenciadorDePilha != null)
    {
        gerenciadorDePilha.pedidoVisual = pedidoAtual;
        gerenciadorDePilha.ConfigurarPedido();
    }

    // Marca o pedido como em andamento
    pedidoEntregue = false;

    // Confirmação de que o pedido foi criado
    if (pedidoAtual != null)
    {
        Debug.Log("Pedido criado com sucesso!");
    }
    else
    {
        Debug.LogError("Falha ao instanciar o pedido!");
    }
}

public void PedidoEntregue()
{
    pedidoEntregue = true; // Permitir que o jogador colete outro pedido
    if (pedidoAtual != null)
    {
        Destroy(pedidoAtual); // Destroi o pedido atual após entrega
        pedidoAtual = null;
    }
    Debug.Log("Pedido entregue e pronto para coletar outro.");
}   

}
