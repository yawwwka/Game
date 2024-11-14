using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class Enemy : Unit
{
    [SerializeField] protected Transform[] targetPoints;
    [SerializeField] protected float speed = 1.0f;
    protected int currentPoint;

    [Space]
    [Header("Timers")]
    [SerializeField] protected float maxWaitTime;
    [SerializeField] protected float minWaitTime;

    protected float waitTime;
    protected float waitTimeCounter;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private EnemyState State
    {
        get { return (EnemyState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Axe axe = collision.GetComponent<Axe>();

        if (axe)
        {
            ReceiveDamage();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPoint = Random.Range(0, targetPoints.Length);

        waitTimeCounter = SetWaitTime();
    }

    // Update is called once per frame
    protected void Update()
    {
        Patrol();      
    }

    protected void Patrol()
    {
        if (transform.position.x == targetPoints[currentPoint].position.x)
        {
            if (waitTimeCounter <= 0)
            {
                State = EnemyState.Patrol;
                IncreaseCurrentPoint();
                waitTimeCounter = SetWaitTime();
            }
            else
            {
                State = EnemyState.Idle;
                waitTimeCounter -= Time.deltaTime;
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPoints[currentPoint].position.x, transform.position.y), speed * Time.deltaTime);
        spriteRenderer.flipX = (targetPoints[currentPoint].position.x - transform.position.x) < 0.0f;
    }

    protected void IncreaseCurrentPoint()
    {
        currentPoint++;
        if (currentPoint >= targetPoints.Length)
        {
            currentPoint = 0;
        }
    }

    protected float SetWaitTime()
    {
        waitTime = Random.Range(minWaitTime, maxWaitTime);

        return waitTime;
    }
}

public enum EnemyState
{
    Idle,
    Patrol
}
