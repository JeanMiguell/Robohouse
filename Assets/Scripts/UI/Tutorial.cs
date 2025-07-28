using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject dialogPanel; // Painel de diálogo principal
    public TextMeshProUGUI dialogText; // Texto do diálogo principal

    public GameObject[] visualPanels; // Array de painéis visuais (um para cada diálogo)

    private string[] dialogues; // Array de diálogos
    private int currentDialogIndex = 0; // Índice do diálogo atual

    void Start()
    {
        // Definir os diálogos
        dialogues = new string[]
        {
            "Parabéns pela sua contratação na EletroSol, Energilson!",
            "Aqui trabalhamos com Pilhas, que são Estrutura de Dados em que as\noperações só acontecem no topo!",
            "Então lembre-se disso ao criar as pilhas de baterias hein!",
            "Sua função aqui é coletar pedidos nessa cabine.",
            "Criar as estruturas na central de comando e programar\n as pilhas de baterias!",
            "Boa sorte em seu trabalho, se precisar de ajuda é só chamar!"
        };

        // Inicializar o painel de diálogo e exibir o primeiro diálogo
        dialogPanel.SetActive(true);
        ShowNextDialog();
    }

    void Update()
    {
        // Avança para o próximo diálogo quando o jogador pressiona a tecla Espaço
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextDialog();
        }
    }

    void ShowNextDialog()
    {
        if (currentDialogIndex < dialogues.Length)
        {
            // Exibe o texto do diálogo atual
            dialogText.text = dialogues[currentDialogIndex];

            // Atualiza os painéis visuais
            AtualizarVisualPanel();

            currentDialogIndex++;
        }
        else
        {
            // Quando os diálogos terminarem, esconde o painel e transita para a próxima cena
            dialogPanel.SetActive(false);
            EsconderTodosVisualPanels();
            LoadRegistroScene();
        }
    }

    void AtualizarVisualPanel()
    {
        // Esconde todos os painéis visuais
        EsconderTodosVisualPanels();

        // Mostra apenas o painel visual correspondente ao diálogo atual
        if (currentDialogIndex < visualPanels.Length)
        {
            visualPanels[currentDialogIndex].SetActive(true);
        }
    }

    void EsconderTodosVisualPanels()
    {
        foreach (GameObject panel in visualPanels)
        {
            panel.SetActive(false);
        }
    }

    void LoadRegistroScene()
    {
        SceneManager.LoadScene("Registro"); // Nome da cena para transição
    }
}
