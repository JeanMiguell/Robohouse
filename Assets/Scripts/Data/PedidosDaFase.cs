using UnityEngine;

[CreateAssetMenu(fileName = "NovaFaseDePedidos", menuName = "Fase/PedidosDaFase")]
public class PedidosDaFase : ScriptableObject
{
    public PedidoData[] pedidosDisponiveis;
}
