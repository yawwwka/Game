using UnityEngine;

public class Egg : Bullet
{
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * speed);
    }
}
