using UnityEngine;

public class EnergilsonMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Velocidade de movimentação
    private Rigidbody2D rb;  // Referência ao Rigidbody2D do personagem
    private Animator animator;  // Referência ao Animator
    private SpriteRenderer spriteRenderer;  // Referência ao SpriteRenderer do personagem

    void Start()
    {
        // Obter as referências do Rigidbody2D, Animator e SpriteRenderer
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Referência ao SpriteRenderer
    }

    void Update()
    {
        // Movimentação para a esquerda e direita
        float moveInput = Input.GetAxisRaw("Horizontal");  // -1 para esquerda, 1 para direita

        // Atualizar a velocidade do Rigidbody2D
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);  // Define a velocidade no eixo X e mantém a velocidade Y

        // Controla a animação: se está se movendo, ativa a animação de correr
        if (moveInput != 0)
        {
            animator.SetBool("isRunning", true);  // Ativa a animação de correr
        }
        else
        {
            animator.SetBool("isRunning", false);  // Desativa a animação de correr
        }

        // Inverter o sprite dependendo da direção
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;  // Roda o sprite para a direita
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;  // Roda o sprite para a esquerda
        }
    }
}
