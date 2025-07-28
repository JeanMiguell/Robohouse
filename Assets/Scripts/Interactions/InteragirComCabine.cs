using UnityEngine;
using RoboHouse.Data;
using RoboHouse.Gameplay;

public class InteragirComCabine : MonoBehaviour
{
    public GameObject canvasCentralDeComando;
    public Transform posicaoCentral;
    public Transform posicaoEntrega;
    public GameObject vitoriaPanel;
    public GameObject derrotaPanel;
    public StructureExecutor structureExecutor;

    private bool pertoDaCabine = false;
    private bool canvasAtivo = false;
    private bool pilhaEntregue = false;
    private bool pedidoEntregueAgora = false;

    void Update()
    {
        if (pertoDaCabine && Input.GetKeyDown(KeyCode.E))
        {
            if (canvasAtivo)
            {
                FecharCanvas();

                if (structureExecutor.VerificarPilha())
                {
                    Debug.Log("Pilha correta! Pronta para entrega.");
                    pedidoEntregueAgora = true;
                }
                else
                {
                    Debug.LogWarning("Pilha incorreta! Tente novamente.");
                    derrotaPanel.SetActive(true);
                }
            }
            else
            {
                if (structureExecutor.PilhaEstaCorreta() && pedidoEntregueAgora)
                {
                    EntregarPilha();
                    pedidoEntregueAgora = false;
                }
                else
                {
                    AbrirCanvas();
                }
            }
        }
    }

    void AbrirCanvas()
    {
        canvasCentralDeComando.SetActive(true);
        canvasAtivo = true;
        Debug.Log("Canvas aberto.");
    }

    void FecharCanvas()
    {
        canvasCentralDeComando.SetActive(false);
        canvasAtivo = false;
        Debug.Log("Canvas fechado.");

        structureExecutor.IsTelaPrincipal = true;

        if (structureExecutor.EstruturaPilha != null)
        {
            structureExecutor.EstruturaPilha.transform.position = structureExecutor.PosicaoBasePrincipal.position;
        }
    }

    void EntregarPilha()
    {
        if (structureExecutor.VerificarPilha())
        {
            Debug.Log("Pilha entregue corretamente!");
            pilhaEntregue = true;
            AlterarCorPedido(Color.green);

            if (structureExecutor.EstruturaPilha != null)
            {
                structureExecutor.EstruturaPilha.transform.position = structureExecutor.PosicaoEntrega.position;
            }

            StartCoroutine(SugarPilha());
        }
        else
        {
            Debug.LogWarning("Pilha entregue incorretamente!");
            AlterarCorPedido(Color.red);
        }
    }

    void AlterarCorPedido(Color cor)
    {
        if (structureExecutor.PedidoVisual != null)
        {
            Renderer renderer = structureExecutor.PedidoVisual.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = cor;
            }
        }
    }

    private System.Collections.IEnumerator SugarPilha()
    {
        float tempo = 1.0f;
        Vector3 posicaoInicial = structureExecutor.EstruturaPilha.transform.position;
        Vector3 posicaoFinal = posicaoInicial + new Vector3(0, 5f, 0);

        float elapsed = 0;
        while (elapsed < tempo)
        {
            structureExecutor.EstruturaPilha.transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, elapsed / tempo);
            elapsed += Time.deltaTime;
            yield return null;
        }

        structureExecutor.EstruturaPilha.transform.position = posicaoFinal;

        Destroy(structureExecutor.EstruturaPilha);
        structureExecutor.Resetar();

        InteragirComBancada interacao = FindObjectOfType<InteragirComBancada>();
        if (interacao != null)
        {
            interacao.PedidoEntregue();
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
}
