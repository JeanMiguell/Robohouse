using System.Collections.Generic;
using UnityEngine;
using RoboHouse.Data;

namespace RoboHouse.Gameplay
{
    public class StructureExecutor : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject prefabBateriaSolar;
        public GameObject prefabBateriaEletrica;
        public GameObject estruturaPilhaPrefab;

        [Header("Posições")]
        [SerializeField] private Transform posicaoBasePilhaPrincipal;
        [SerializeField] private Transform posicaoBasePilhaProgramacao;
        [SerializeField] private Transform posicaoEntrega;

        [Header("Estado")]
        [SerializeField] private bool isTelaPrincipal = true;

        private Stack<GameObject> pilhaVisual = new();
        private Stack<TipoBateria> pilhaLogica = new();
        private GameObject estruturaPilha;
        private Transform posicaoBaseAtual;
        private float alturaBateria = 3f;

        private bool pilhaCriada = false;
        private bool memoriaAlocada = false;
        private bool pilhaCorreta = false;
        private List<string> pedidoAtual;

        // 🔧 Propriedade usada externamente para validar o pedido
        public GameObject PedidoVisual { get; set; }

        // ✅ Acesso externo para InteragirComCabine
        public GameObject EstruturaPilha => estruturaPilha;
        public Transform PosicaoEntrega => posicaoEntrega;
        public Transform PosicaoBasePrincipal => posicaoBasePilhaPrincipal;
        public bool IsTelaPrincipal { get => isTelaPrincipal; set => isTelaPrincipal = value; }

        private void AtualizarPosicaoBase()
        {
            posicaoBaseAtual = isTelaPrincipal ? posicaoBasePilhaPrincipal : posicaoBasePilhaProgramacao;
        }

        public void CriarPilha()
{
    AtualizarPosicaoBase(); // Isso está ótimo

    if (!pilhaCriada && estruturaPilhaPrefab != null && posicaoBaseAtual != null)
    {
        estruturaPilha = Instantiate(estruturaPilhaPrefab, posicaoBaseAtual.position, Quaternion.identity);
        estruturaPilha.transform.SetParent(transform);
        pilhaCriada = true;
        Debug.Log("Estrutura da pilha criada.");
    }
    else
    {
        Debug.LogWarning("A pilha já foi criada ou falta configuração.");
    }
}

        public void AlocarMemoria()
        {
            if (!pilhaCriada)
            {
                Debug.LogWarning("Crie a estrutura da pilha antes de alocar memória.");
                return;
            }

            memoriaAlocada = true;
            Debug.Log("Memória alocada!");
        }

        public void EmpilharBateria(TipoBateria tipo)
        {
            Debug.Log($"[StructureExecutor] Empilhando bateria do tipo: {tipo}");


            GameObject prefab = tipo switch
            {
                TipoBateria.Solar => prefabBateriaSolar,
                TipoBateria.Eletrica => prefabBateriaEletrica,
                _ => null
            };

            if (prefab == null)
            {
                Debug.LogWarning($"[StructureExecutor] Prefab nulo para tipo: {tipo}");
                return;
            }

            if (!memoriaAlocada || estruturaPilha == null)
            {
                Debug.LogWarning("[StructureExecutor] Memória não alocada ou estrutura da pilha inexistente!");
                return;
            }

            GameObject novaBateria = Instantiate(prefab, estruturaPilha.transform);

            Vector3 posicao = estruturaPilha.transform.position + new Vector3(0, pilhaVisual.Count * alturaBateria, 0);
            novaBateria.transform.position = posicao;
            pilhaVisual.Push(novaBateria);
            pilhaLogica.Push(tipo);
            Debug.Log($"[StructureExecutor] {tipo} empilhada na posição: {posicao}");
    Debug.Log($"[Debug] Nome da bateria criada: {novaBateria.name}");
Debug.Log($"[Debug] Posição world: {novaBateria.transform.position}");
Debug.Log($"[Debug] SpriteRenderer: {novaBateria.GetComponent<SpriteRenderer>()?.sprite}");
 
}

        public void ConfigurarPedido(List<string> baterias)
        {
            pedidoAtual = baterias;
            pilhaCorreta = false;
            Debug.Log("Pedido configurado: " + string.Join(", ", baterias));
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

        public bool PilhaEstaCorreta()
        {
            return pilhaCorreta;
        }

        public void Resetar()
        {
            foreach (var b in pilhaVisual)
                Destroy(b);

            pilhaVisual.Clear();
            pilhaLogica.Clear();
            estruturaPilha = null;
            pedidoAtual = null;
            PedidoVisual = null;
            pilhaCriada = false;
            memoriaAlocada = false;
            pilhaCorreta = false;
        }
    }
}
