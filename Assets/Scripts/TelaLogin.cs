using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Certifique-se de incluir isso

public class TelaLogin : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI feedbackText;  // Para exibir erros de login

    // Função chamada pelo botão de login
    public void TentarLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        
        if (username == "admin" && password == "1234") // Exemplo de verificação (substitua por uma lógica real)
        {
            SceneManager.LoadScene("MainScene");  // Vá para a cena principal do jogo
        }
        else
        {
            Debug.Log("Erro ao realizar login");
        }
    }

    // Função para ir à tela de registro
    public void IrParaRegistro()
    {
        SceneManager.LoadScene("Registro");
    }
}
