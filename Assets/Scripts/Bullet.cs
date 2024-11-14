using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject parent;
    public GameObject Parent { set { parent = value; } }

    protected float speed = 3f;
    protected Vector3 direction;
    public Vector3 Direction { set { direction = value; } }

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.GetComponent<Unit>();

        if (unit && unit.gameObject != parent)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Block")
            Destroy(gameObject);
    }
}
