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

    private Stack<TipoBateria> pilhaLogica = new Stack<TipoBateria>();

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

    public void ConfigurarPedido(List<string> baterias)
{
    pedidoAtual = baterias;
    pilhaCorreta = false;
    Debug.Log("Pedido configurado dinamicamente: " + string.Join(", ", baterias));
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
        // Define o tipo lógico
        TipoBateria tipoBateria;

        switch (tipo)
        {
            case "Solar":
                tipoBateria = TipoBateria.Solar;
                break;
            case "Elétrica":
                tipoBateria = TipoBateria.Eletrica;
                break;
            case "Nuclear":
                tipoBateria = TipoBateria.Nuclear;
                break;
            default:
                Debug.LogWarning("Tipo inválido para pilha lógica.");
                return;
        }

        // Salva na pilha lógica (validação)
        pilhaLogica.Push(tipoBateria);

        // Salva na pilha visual (sprites)
        novaBateria.transform.SetParent(estruturaPilha.transform);

        Vector3 posicaoInicial = posicaoBaseAtual.position + new Vector3(0, 2f, 0);
        novaBateria.transform.position = posicaoInicial;

        Vector3 posicaoFinal = posicaoBaseAtual.position + new Vector3(0, pilhaDeBaterias.Count * alturaBateria, 0);

        pilhaDeBaterias.Push(novaBateria);

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
    if (pedidoAtual == null || pilhaLogica.Count != pedidoAtual.Count)
        return false;

    TipoBateria[] baterias = pilhaLogica.ToArray();

    for (int i = 0; i < baterias.Length; i++)
    {
        if (baterias[i].ToString() != pedidoAtual[i])
        {
            Debug.Log($"Erro na posição {i}: esperado {pedidoAtual[i]}, recebido {baterias[i]}");
            return false;
        }
    }

    pilhaCorreta = true;
    return true;
}

public void ResetarTudo()
{
    foreach (var bateria in pilhaDeBaterias)
        Destroy(bateria);

    pilhaDeBaterias.Clear();
    pilhaLogica.Clear();

    estruturaPilha = null;
    pedidoAtual = null;
    pedidoVisual = null;

    pilhaCriada = false;
    memoriaAlocada = false;
    pilhaCorreta = false;

    Debug.Log("Reset geral concluído.");
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
