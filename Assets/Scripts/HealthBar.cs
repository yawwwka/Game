using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform[] hearts = new Transform[5];

    private Character character;
    private void Awake()
    {
        character = FindAnyObjectByType<Character>();

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    public void Refresh()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < character.Health)
                hearts[i].gameObject.SetActive(true);
            else
                hearts[i].gameObject.SetActive(false);
        }
    }
}
