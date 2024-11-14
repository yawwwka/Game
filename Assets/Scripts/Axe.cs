using UnityEngine;

public class Axe : Bullet
{
    private Axe[] axes;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * speed);
    }
}