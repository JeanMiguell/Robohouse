using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public AudioSource musicaDeFundo; // Referência ao áudio de fundo
    public AudioSource efeitosSonoros; // Referência para tocar os efeitos sonoros
    public AudioClip somCliqueBotao; // Efeito sonoro ao clicar nos botões
    public GameObject painelOpcoes; // Painel de opções
    private bool musicaAtiva = true; // Controle do estado da música

    GLBoard gLBoard;

    private void Start()
    {
        // Tocar a música de fundo ao iniciar o menu
        if (musicaDeFundo != null && !musicaDeFundo.isPlaying)
        {
            musicaDeFundo.loop = true; // Define a música para repetir
            musicaDeFundo.Play();
        }
    }

    // Função chamada ao clicar no botão "Start"
    async void IniciarJogo()
    {
        TocarSomClique();
        GLBoard gLBoard = new GLBoard("MxsgS8uiKX23BTS3VLeXDw", SystemInfo.deviceUniqueIdentifier);
        await gLBoard.LOAD_USER_DATA();
        gLBoard.SetCustomReport("Esse jogador tem dificuldades com as coisas"); // Toca o efeito sonoro do botão
        StartCoroutine(gLBoard.SEND_USER_DATA());
        SceneManager.LoadScene("Login"); // Carrega a cena "Login"
    }

    // Função chamada ao clicar no botão "Sair"
    public void SairDoJogo()
    {
        TocarSomClique(); // Toca o efeito sonoro do botão
        Application.Quit(); // Fecha o jogo

        // Exibe uma mensagem no Console, útil apenas no editor
        Debug.Log("O jogo foi encerrado.");
    }

    // Função chamada ao clicar no botão "Opções"
    public void AbrirOpcoes()
    {
        TocarSomClique(); // Toca o efeito sonoro do botão

        if (painelOpcoes != null)
        {
            painelOpcoes.SetActive(true); // Exibe o painel de opções
        }
        else
        {
            Debug.LogWarning("Painel de opções não configurado no Inspector.");
        }
    }

    // Função chamada ao clicar no botão para fechar o painel de opções
    public void FecharOpcoes()
    {
        TocarSomClique(); // Toca o efeito sonoro do botão

        if (painelOpcoes != null)
        {
            painelOpcoes.SetActive(false); // Esconde o painel de opções
        }
    }

    // Função para tocar o som de clique
    private void TocarSomClique()
{
    if (efeitosSonoros != null && somCliqueBotao != null)
    {
        Debug.Log("Tocando som de clique..."); // Verificar se está chamando
        efeitosSonoros.PlayOneShot(somCliqueBotao); // Toca o som do botão
    }
    else
    {
        Debug.LogWarning("Efeitos sonoros ou som de clique não configurados no Inspector.");
    }
}

public void AlternarMusica()
    {
        if (musicaDeFundo != null)
        {
            musicaAtiva = !musicaAtiva; // Alterna o estado da música
            musicaDeFundo.mute = !musicaAtiva; // Ativa ou desativa o som
            Debug.Log("Música de fundo " + (musicaAtiva ? "ativada" : "desativada"));
        }
        else
        {
            Debug.LogWarning("Música de fundo não configurada no Inspector.");
        }
    }
}
