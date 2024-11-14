using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private Transform target;

    private void Awake()
    {
        if (target == null)
            target = FindAnyObjectByType<Character>().transform;
    }

    public void Update()
    {
        Vector3 position = target.position;
        position.z = -10.0f;
        position.y += 0.4f;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
