using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class TelaLogin : MonoBehaviour
{
    public TMP_InputField usernameInput;      // Corrigido: agora está declarado
    public TMP_InputField passwordInput;
    public TextMeshProUGUI feedbackText;

    GLBoard gLBoard;

    public async Task TentarLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username == "admin" && password == "1234")
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            feedbackText.text = "Usuário ou senha incorretos.";
            Debug.Log("Erro ao realizar login");
        }
    }

    public void IrParaRegistro()
    {
        SceneManager.LoadScene("Registro");
    }
}
