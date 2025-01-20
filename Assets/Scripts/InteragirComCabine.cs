using UnityEngine;

public class InteragirComCabine : MonoBehaviour
{
    public GameObject canvasCentralDeComando;  // Referência ao Canvas
    public Transform posicaoCentral;          // Posição da central na tela principal
    public Transform posicaoEntrega;          // Posição onde a pilha será "sugada"
    public GameObject vitoriaPanel;           // Painel de vitória
    public GameObject derrotaPanel;           // Painel de derrota

    private bool pertoDaCabine = false;       // Verifica se o jogador está perto da cabine
    private bool canvasAtivo = false;         // Verifica se o Canvas está ativo
    private bool pilhaEntregue = false;       // Controle de entrega da pilha
    public GerenciadorDePilha gerenciadorDePilha;

    void Update()
    {
        if (pertoDaCabine && Input.GetKeyDown(KeyCode.E))
        {
            if (canvasAtivo)
            {
                FecharCanvas();

                // Verificar se a pilha está correta antes de sair
                if (gerenciadorDePilha.VerificarPilha())
                {
                    Debug.Log("Pilha correta! Verificando posição ao retornar à tela principal.");
                    Debug.Log($"Estrutura da pilha posição atual: {gerenciadorDePilha.EstruturaPilha.transform.position}");
                }
                else
                {
                    Debug.LogWarning("Pilha incorreta! Tente novamente.");
                    derrotaPanel.SetActive(true);
                }
            }
            else
            {
                // Abrir o Canvas apenas se a pilha ainda não foi criada ou entregue
                if (!pilhaEntregue && !gerenciadorDePilha.PilhaEstaCorreta())
                {
                    AbrirCanvas();
                }
                else if (gerenciadorDePilha.PilhaEstaCorreta())
                {
                    // Sugação da pilha
                    EntregarPilha();
                }
                else
                {
                    Debug.LogWarning("Memória não alocada ou estrutura da pilha não criada!");
                }
            }
        }
    }

    // Método para abrir o Canvas
    void AbrirCanvas()
    {
        canvasCentralDeComando.SetActive(true);
        canvasAtivo = true;
        Debug.Log("Canvas aberto.");
    }

    // Método para fechar o Canvas
    void FecharCanvas()
    {
        canvasCentralDeComando.SetActive(false);
        canvasAtivo = false;
        Debug.Log("Canvas fechado.");
    }

    // Entregar a pilha (sugada para a posição de entrega)
    void EntregarPilha()
{
    if (gerenciadorDePilha.VerificarPilha())
    {
        Debug.Log("Pilha entregue corretamente!");
        pilhaEntregue = true;

        // Alterar cor do pedido para verde
        AlterarCorPedido(Color.green);

        // Garante que a estrutura mantenha a posição correta antes da animação
        if (gerenciadorDePilha.EstruturaPilha != null)
        {
            gerenciadorDePilha.EstruturaPilha.transform.position = gerenciadorDePilha.posicaoEntrega.position;
        }

        // Inicia a animação da pilha sendo sugada
        StartCoroutine(SugarPilha());
    }
    else
    {
        Debug.LogWarning("Pilha entregue incorretamente!");
        AlterarCorPedido(Color.red);
    }
}

    // Alterar a cor do pedido
    void AlterarCorPedido(Color cor)
    {
        if (gerenciadorDePilha.pedidoVisual != null)
        {
            Renderer renderer = gerenciadorDePilha.pedidoVisual.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = cor;
            }
        }
    }

    // Animação da pilha sendo sugada
    private System.Collections.IEnumerator SugarPilha()
    {
        float tempo = 1.0f; // Tempo da animação
        Vector3 posicaoInicial = gerenciadorDePilha.EstruturaPilha.transform.position;
        Vector3 posicaoFinal = new Vector3(posicaoInicial.x, posicaoInicial.y + 5f, posicaoInicial.z); // Move 5 unidades para cima

        float elapsed = 0;
        while (elapsed < tempo)
        {
            // Anima apenas o eixo Y (posição final mais alta)
            gerenciadorDePilha.EstruturaPilha.transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, elapsed / tempo);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Garante que a posição final seja exatamente a correta
        gerenciadorDePilha.EstruturaPilha.transform.position = posicaoFinal;

        // Após a animação, destrói a estrutura
        Destroy(gerenciadorDePilha.EstruturaPilha);
        Debug.Log("Estrutura da pilha sugada e removida.");
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
}