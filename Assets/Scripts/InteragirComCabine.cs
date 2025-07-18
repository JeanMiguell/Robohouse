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
    private bool pilhaEntregue = false;   
    private bool pedidoEntregueAgora = false;
    // Controle de entrega da pilha
    public GerenciadorDePilha gerenciadorDePilha;

    void Update()
{
    if (pertoDaCabine && Input.GetKeyDown(KeyCode.E))
    {
        if (canvasAtivo)
        {
            FecharCanvas();

            // Verifica se a pilha montada está correta
            if (gerenciadorDePilha.VerificarPilha())
            {
                Debug.Log("Pilha correta! Pronta para entrega.");
                pedidoEntregueAgora = true; // Marca que pode entregar
            }
            else
            {
                Debug.LogWarning("Pilha incorreta! Tente novamente.");
                derrotaPanel.SetActive(true);
            }
        }
        else
        {
            // Se a pilha estiver correta e ainda não foi entregue, entrega agora
            if (gerenciadorDePilha.PilhaEstaCorreta() && pedidoEntregueAgora)
            {
                EntregarPilha();
                pedidoEntregueAgora = false; // Reseta a flag após entrega
            }
            else
            {
                // Caso contrário, permite montar ou refazer a pilha
                AbrirCanvas();
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

    gerenciadorDePilha.isTelaPrincipal = true;

    // Aqui força o reposicionamento da pilha
    if (gerenciadorDePilha.EstruturaPilha != null)
    {
        gerenciadorDePilha.EstruturaPilha.transform.position = gerenciadorDePilha.posicaoBasePilhaPrincipal.position;
    }
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
    float tempo = 1.0f;
    Vector3 posicaoInicial = gerenciadorDePilha.EstruturaPilha.transform.position;
    Vector3 posicaoFinal = new Vector3(posicaoInicial.x, posicaoInicial.y + 5f, posicaoInicial.z);

    float elapsed = 0;
    while (elapsed < tempo)
    {
        gerenciadorDePilha.EstruturaPilha.transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, elapsed / tempo);
        elapsed += Time.deltaTime;
        yield return null;
    }

    gerenciadorDePilha.EstruturaPilha.transform.position = posicaoFinal;

    // Destroi a estrutura visual
    Destroy(gerenciadorDePilha.EstruturaPilha);
    Debug.Log("Estrutura da pilha sugada e removida.");

    // ✅ Resetar estado interno do Gerenciador
    gerenciadorDePilha.ResetarTudo();

    // ✅ Informar que o pedido foi entregue e liberar o próximo
    InteragirComBancada interacao = FindObjectOfType<InteragirComBancada>();
    if (interacao != null)
    {
        interacao.PedidoEntregue();
    }

    // (Opcional) Mostrar painel de vitória
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