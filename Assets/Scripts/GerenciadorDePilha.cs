using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDePilha : MonoBehaviour
{
    public GameObject prefabBateriaSolar;
    public GameObject prefabBateriaEletrica;
    public Transform posicaoBasePilhaPrincipal;
    public Transform posicaoBasePilhaProgramacao;
    public Transform posicaoEntrega;
    public GameObject estruturaPilhaPrefab;
    public GameObject pedidoVisual; // Adicionado o campo pedidoVisual

    private Stack<GameObject> pilhaDeBaterias = new Stack<GameObject>();
    private GameObject estruturaPilha;

    private bool pilhaCriada = false;
    private bool memoriaAlocada = false;
    private bool pilhaCorreta = false;
    public AudioSource musicaDeFundo; // Referência ao áudio de fundo

    // Propriedade para acessar a estrutura da pilha
public GameObject EstruturaPilha => estruturaPilha;

    private float alturaBateria = 3f;
    private List<string> pedidoAtual;

    public bool isTelaPrincipal = true;

    private Transform posicaoBaseAtual;

    private void Start()
    {
        // Tocar a música de fundo ao iniciar o menu
        if (musicaDeFundo != null && !musicaDeFundo.isPlaying)
        {
            musicaDeFundo.loop = true; // Define a música para repetir
            musicaDeFundo.Play();
        }
    }

    private void AtualizarPosicaoBase()
    {
        posicaoBaseAtual = isTelaPrincipal ? posicaoBasePilhaPrincipal : posicaoBasePilhaProgramacao;
    }

    public void CriarPilha()
    {
        AtualizarPosicaoBase();

        if (!pilhaCriada && estruturaPilhaPrefab != null && posicaoBaseAtual != null)
        {
            estruturaPilha = Instantiate(estruturaPilhaPrefab, posicaoBaseAtual.position, Quaternion.identity);
            estruturaPilha.transform.SetParent(transform);
            pilhaCriada = true;
            Debug.Log("Estrutura da pilha criada na posição: " + posicaoBaseAtual.position);
        }
        else
        {
            Debug.LogWarning("A pilha já foi criada ou falta configuração.");
        }
    }

    public void ConfigurarPedido()
    {
        pedidoAtual = new List<string> { "BateriaEletrica", "BateriaSolar" };
        Debug.Log($"Pedido configurado: {string.Join(", ", pedidoAtual)}");
    }

    public void AlocarMemoria()
    {
        if (pilhaCriada)
        {
            memoriaAlocada = true;
            Debug.Log("Memória alocada! Push habilitado.");
        }
        else
        {
            Debug.LogWarning("Crie a estrutura da pilha antes de alocar memória.");
        }
    }

    public void EmpilharBateria(string tipo)
{
    if (!memoriaAlocada)
    {
        Debug.LogWarning("Memória não alocada! Use o comando 'Alocar Memória' antes de dar Push.");
        return;
    }

    AtualizarPosicaoBase();

    GameObject novaBateria = tipo switch
    {
        "Solar" => Instantiate(prefabBateriaSolar),
        "Elétrica" => Instantiate(prefabBateriaEletrica),
        _ => null
    };

    if (novaBateria != null)
    {
        novaBateria.transform.SetParent(estruturaPilha.transform);

        // Posição inicial elevada para a queda
        Vector3 posicaoInicial = posicaoBaseAtual.position + new Vector3(0, 2f, 0); // Eleva 2 unidades no eixo Y
        novaBateria.transform.position = posicaoInicial;

        // Calcula a posição final com base na pilha
        Vector3 posicaoFinal = posicaoBaseAtual.position + new Vector3(0, pilhaDeBaterias.Count * alturaBateria, 0);

        // Atualiza a pilha antes da animação
        pilhaDeBaterias.Push(novaBateria);

        // Inicia o efeito de queda
        StartCoroutine(EfeitoDeQueda(novaBateria, posicaoFinal));

        Debug.Log($"Bateria {tipo} empilhada na posição final: {posicaoFinal}");
    }
    else
    {
        Debug.LogWarning($"Tipo de bateria inválido: {tipo}");
    }
}


    public bool VerificarPilha()
{
    if (pedidoAtual == null)
    {
        Debug.LogWarning("Nenhum pedido configurado!");
        return false;
    }

    if (pilhaDeBaterias.Count != pedidoAtual.Count)
    {
        Debug.Log("A pilha não corresponde ao pedido: tamanhos diferentes.");
        return false;
    }

    GameObject[] baterias = pilhaDeBaterias.ToArray();
    for (int i = 0; i < baterias.Length; i++)
    {
        string tipoBateria = baterias[i].name.Replace("(Clone)", "").Trim(); // Remove "(Clone)" do nome
        if (tipoBateria != pedidoAtual[i])
        {
            Debug.Log($"Erro na posição {i}: esperado {pedidoAtual[i]}, recebido {tipoBateria}.");
            return false;
        }
    }

    Debug.Log("A pilha criada corresponde ao pedido!");
    pilhaCorreta = true;

    // Após verificar, destruímos o pedido visual
    if (pedidoVisual != null)
    {
        Destroy(pedidoVisual);
        pedidoVisual = null;
    }

    return true;
}




    public void LimparPilha()
    {
        foreach (var bateria in pilhaDeBaterias)
        {
            Destroy(bateria);
        }
        pilhaDeBaterias.Clear();
        pilhaCriada = false;
        Debug.Log("Pilha limpa!");
    }

    public bool PilhaEstaCorreta()
    {
        return pilhaCorreta;
    }

    private System.Collections.IEnumerator EfeitoDeQueda(GameObject bateria, Vector3 posicaoFinal)
    {
        float tempo = 0.3f;
        Vector3 posicaoInicial = bateria.transform.position + new Vector3(0, 2f, 0);
        bateria.transform.position = posicaoInicial;

        float elapsed = 0;
        while (elapsed < tempo)
        {
            bateria.transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, elapsed / tempo);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bateria.transform.position = posicaoFinal;
    }

    
}
