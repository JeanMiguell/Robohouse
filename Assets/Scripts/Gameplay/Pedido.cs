using RoboHouse.Gameplay;
using UnityEngine;
using RoboHouse.Data;
using System.Collections.Generic;

public class Pedido : MonoBehaviour
{
    public StructureExecutor structureExecutor;

    public Sprite bateriaEletrica;
    public Sprite bateriaSolar;
    public Sprite bateriaNuclear;

    public List<TipoBateria> tiposBaterias;  // lista com os tipos do pedido

    void Start()
    {
        CriarBaterias();
    }

    void CriarBaterias()
    {
        AddBateriasAoPedido();
    }

    void AddBateriasAoPedido()
    {
        float alturaInicial = 0.7f;
        float deslocamento = -1.2f;
        float alturaAtual = alturaInicial;

        Vector3 escalaBateria = new Vector3(0.6f, 0.65f, 1);

        foreach (TipoBateria tipo in tiposBaterias)
        {
            GameObject bateria = new GameObject("Bateria");
            bateria.transform.parent = transform;
            bateria.transform.position = transform.position + new Vector3(0, alturaAtual, 0);

            SpriteRenderer spriteRenderer = bateria.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 10;

            switch (tipo)
            {
                case TipoBateria.Eletrica:
                    spriteRenderer.sprite = bateriaEletrica;
                    break;
                case TipoBateria.Solar:
                    spriteRenderer.sprite = bateriaSolar;
                    break;
                case TipoBateria.Nuclear:
                    spriteRenderer.sprite = bateriaNuclear;
                    break;
            }

            bateria.transform.localScale = escalaBateria;
            alturaAtual += deslocamento;
        }
    }
}
