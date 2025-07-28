using UnityEngine;

public class ComandoResetController : MonoBehaviour
{
    public void ResetarTodosBlocos()
    {
        Arrastavel[] blocos = FindObjectsOfType<Arrastavel>();
        foreach (var bloco in blocos)
        {
            bloco.ResetarPosicao();
        }
    }
}