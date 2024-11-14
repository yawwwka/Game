using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private int health = 5;
    [SerializeField] private float jumpForce = 3.0f;
    [SerializeField] private float damageCooldown = 3.0f;
    [SerializeField] private float axeRestoreCooldown = 3.0f;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float retreatDistance = 2.0f;

    public bool InDialogue { get; set; }
    private float lastDamageTime;
    private float lastThrowTime;

    private int currentAxeCount = 3;

    public int AxesCount
    {
        get { return currentAxeCount; }
        set
        {
            if (value <= 3) currentAxeCount = value;
            axesBar.Refresh();
        }
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (value <= 5) health = value;
            healthBar.Refresh();
        }
    }

    private HealthBar healthBar;
    private AxesBar axesBar;
    private DialogueSystem dialogue;

    private bool isGrounded = false;
    private Axe axe;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        healthBar = FindAnyObjectByType<HealthBar>();
        axesBar = FindAnyObjectByType<AxesBar>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        axe = Resources.Load<Axe>("Axe");
        dialogue = Resources.Load<DialogueSystem>("DialogueBackground");
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded) State = CharState.Idle;

        if (Input.GetButton("Horizontal") && !InDialogue) Run();
        if (isGrounded && Input.GetButtonDown("Jump") && !InDialogue) Jump();
        if (Input.GetButtonDown("Fire1") && !InDialogue) Throw();

        if (transform.position.x <= targetPosition.x)
        {
            StartCoroutine(HandlePlayerControl());
        }
    }

    private void Run()
    {
        if (isGrounded) State = CharState.Run;

        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        spriteRenderer.flipX = direction.x < 0.0f;
    }

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        State = CharState.Jump;
    }

    public void Throw()
    {
        if (AxesCount <= 0 || InDialogue) return;

        AxesCount--;
        StartCoroutine(ThrowAxeCoroutine());
    }

    private IEnumerator ThrowAxeCoroutine()
    {
        State = CharState.Throw;
        animator.SetTrigger("Throw");
        yield return new WaitForSeconds(0.35f);

        Vector3 position = transform.position;
        position.x += (spriteRenderer.flipX ? -0.1f : 0.1f);

        Axe newThrow = Instantiate(axe, position, axe.transform.rotation);
        newThrow.Parent = gameObject;
        newThrow.Direction = newThrow.transform.right * (spriteRenderer.flipX ? -1.0f : 1.0f);

        StartCoroutine(RestoreAxe());
    }

    public override void ReceiveDamage()
    {
        if (Time.time - lastDamageTime < damageCooldown) return;

        lastDamageTime = Time.time;
        Health--;
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.AddForce(transform.up * 2.0f, ForceMode2D.Impulse);
        rigidbody.AddForce(transform.right * 2.0f * (spriteRenderer.flipX ? 1.0f : -1.0f), ForceMode2D.Impulse);

        if (health == 0)
        {
            Die();
        }
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        isGrounded = colliders.Length > 1;

        if (!isGrounded)
            State = CharState.Jump;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Unit>())
        {
            ReceiveDamage();
        }
        else if (collision.GetComponent<Water>())
        {
            ReceiveDamage();
        }
        else if (collision.GetComponent<Egg>())
        {
            ReceiveDamage();
        }
        else if (collision.CompareTag("ChickenTrigger"))
        {
            DialogueSystem newDialogue = Instantiate(dialogue, FindFirstObjectByType<Canvas>().transform);
            newDialogue.Lines = new string[3];
            newDialogue.Lines[0] = "Игорян: Мы заходим на территорию куриц, агрессивно к нам настроеных.";
            newDialogue.Lines[1] = "Игорян: Если они смогут дотронуться до тебя - ты потеряешь единицу своего здоровья.";
            newDialogue.Lines[2] = "Игорян: Тебе нужно убить их раньше, чем они доберутся до тебя. (Кинуть топор - ЛКМ/Ctrl)";
        }
    }

    private IEnumerator HandlePlayerControl()
    {
        InDialogue = true;
        yield return StartCoroutine(Retreat());
        InDialogue = false;
    }

    private IEnumerator Retreat()
    {
        Vector3 originalPosition = transform.position;
        Vector3 retreatPosition = originalPosition + new Vector3(retreatDistance, 0, 0);

        while (transform.position.x < retreatPosition.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, retreatPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator RestoreAxe()
    {
        yield return new WaitForSeconds(axeRestoreCooldown);
        AxesCount++;
    }
}

public enum CharState
{
    Idle,
    Run,
    Jump,
    Throw
}
