using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     // Horizontal movement speed
    public float jumpForce = 10f;    // Jump strength
    public Transform groundCheck;    // Empty GameObject at feet
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform visual;
    private Animator anim;

    public bool wasGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = visual.GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Checar se está tocando o chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. Movimento Horizontal (A/D ou Setas Esquerda/Direita)
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 3. Animação de Andar (Run)
        anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0f && isGrounded);
        if (moveInput > 0.01f)
        {
            visual.localScale = new Vector3(4, 4, 4); // Vira para a direita
        }
        else if (moveInput < -0.01f)
        {
            visual.localScale = new Vector3(-4, 4, 4); // Vira para a esquerda
        }

        // 4. Lógica e Animação de Pulo
        // Verifica se PULAR (Seta para Cima OU W) foi pressionado E se está no chão
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
            anim.SetBool("isJumping", true); // Define o parâmetro de pulo como TRUE
        }

        // 5. Atualiza o parâmetro de pulo no Animator
        // Se estiver no chão, o pulo deve ser FALSE (independente de ter acabado de pular)
        if (!isGrounded)
        {
            wasGrounded = true;
            
        }
        if (wasGrounded)
        {
            anim.SetBool("isJumping", true);
        }
        if(wasGrounded && isGrounded)
        {
            wasGrounded = false;
            anim.SetBool("isJumping", false);
        }
        // 6. Lógica de Luta (FightPose e Punch)
        anim.SetBool("isFightPose", Input.GetKey(KeyCode.LeftShift) && isGrounded);
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            rb.linearVelocity = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.Z))
            {
                anim.SetTrigger("Punch");
            }
        }
    }
}