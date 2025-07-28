using UnityEngine;
using RoboHouse.Data;
using RoboHouse.Gameplay;

public class FeedbackUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public StructureExecutor structureExecutor;
    void Start()
    {
    structureExecutor.CriarPilha();
    structureExecutor.AlocarMemoria();
    structureExecutor.EmpilharBateria(TipoBateria.Solar);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
