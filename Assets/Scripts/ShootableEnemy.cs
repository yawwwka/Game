using UnityEngine;

public class ShootableEnemy : Enemy
{
    [SerializeField] private float rate = 2.0f;

    private Egg egg;

    private void Awake()
    {
        egg = Resources.Load<Egg>("Egg");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating("Shoot", rate, rate);
    }

    private void Shoot()
    {
        Vector3 position = transform.position;
        position.y -= 0.3f;
        Egg newEgg = Instantiate(egg, position, egg.transform.rotation);

        newEgg.Parent = gameObject;
        newEgg.Direction = -newEgg.transform.up;
    }
}
