using UnityEngine;

public enum TipoBateria
{
    Eletrica,
    Solar,
    Nuclear
}

public class Pedido : MonoBehaviour
{
    public TipoBateria[] tiposDeBaterias; // Array para armazenar os tipos de baterias no pedido

    public Sprite bateriaEletrica; // Sprite para a bateria elétrica
    public Sprite bateriaSolar;    // Sprite para a bateria solar
    public Sprite bateriaNuclear;  // Sprite para a bateria nuclear

    void Start()
    {
        CriarBaterias();
    }

    void CriarBaterias()
    {
        AddBateriasAoPedido();
    }

    // Método para adicionar baterias ao pedido de forma dinâmica
    void AddBateriasAoPedido()
    {
        float alturaInicial = 0.7f; // Altura inicial para a primeira bateria
        float deslocamento = -1.2f; // Distância entre cada bateria
        float alturaAtual = alturaInicial;

        Vector3 escalaBateria = new Vector3(0.6f, 0.65f, 1); // Ajuste a escala conforme necessário

        // Loop para gerar as baterias conforme os tipos no pedido
        foreach (TipoBateria tipo in tiposDeBaterias)
        {
            GameObject bateria = new GameObject("Bateria");
            bateria.transform.parent = transform;  // Fazer a bateria ser filha do pedido
            bateria.transform.position = transform.position + new Vector3(0, alturaAtual, 0);

            // Adicionar um componente de SpriteRenderer para cada bateria
            SpriteRenderer spriteRenderer = bateria.AddComponent<SpriteRenderer>();

            spriteRenderer.sortingOrder = 10; // Número alto para garantir que fique na frente

            // Definir o tipo de bateria e sua imagem
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

            // Ajusta a escala da bateria
            bateria.transform.localScale = escalaBateria;

            // Atualiza a altura para a próxima bateria na pilha
            alturaAtual += deslocamento;
        }
    }
}
