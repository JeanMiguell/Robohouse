using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DerrotaUIController : MonoBehaviour
{
    public Button buttonRecomecar;
    public Button buttonVoltar;

    private void Start()
    {
        buttonRecomecar.onClick.AddListener(RecomecarFase);
        buttonVoltar.onClick.AddListener(VoltarMenu);
    }

    public void RecomecarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicial"); // ou a cena correta
    }
}
