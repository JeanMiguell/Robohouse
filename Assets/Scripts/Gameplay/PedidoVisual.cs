using UnityEngine;
using RoboHouse.Data;

namespace RoboHouse.Gameplay
{
    public class PedidoVisual : MonoBehaviour
    {
        [Header("Sprites de Bateria")]
        public Sprite bateriaEletrica;
        public Sprite bateriaSolar;
        public Sprite bateriaNuclear;

        [Header("Configuração do Pedido")]
        public TipoBateria[] tiposDeBaterias;

        [Header("Visual")]
        public Vector3 escalaBateria = new(0.6f, 0.65f, 1);
        public float alturaInicial = 0.7f;
        public float deslocamento = -1.2f;

        private void Start()
        {
            GerarVisualDoPedido();
        }

        public void GerarVisualDoPedido()
        {
            float alturaAtual = alturaInicial;

            foreach (TipoBateria tipo in tiposDeBaterias)
            {
                GameObject bateriaGO = new GameObject($"Bateria_{tipo}");
                bateriaGO.transform.SetParent(transform);
                bateriaGO.transform.position = transform.position + new Vector3(0, alturaAtual, 0);

                SpriteRenderer spriteRenderer = bateriaGO.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = ObterSpritePorTipo(tipo);
                spriteRenderer.sortingOrder = 10;

                bateriaGO.transform.localScale = escalaBateria;

                alturaAtual += deslocamento;
            }
        }

        private Sprite ObterSpritePorTipo(TipoBateria tipo)
        {
            return tipo switch
            {
                TipoBateria.Eletrica => bateriaEletrica,
                TipoBateria.Solar => bateriaSolar,
                TipoBateria.Nuclear => bateriaNuclear,
                _ => null
            };
        }
    }
}
