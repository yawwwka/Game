using UnityEngine;

public class AxesBar : MonoBehaviour
{
    private Transform[] axes = new Transform[3];

    private Character character;

    private void Awake()
    {
        character = FindAnyObjectByType<Character>();

        for (int i = 0; i < axes.Length; i++)
        {
            axes[i] = transform.GetChild(i);
        }
    }
    public void Refresh()
    {
        for (int i = 0; i < axes.Length; i++)
        {
            if (i < character.AxesCount)
                axes[i].gameObject.SetActive(true);
            else
                axes[i].gameObject.SetActive(false);
        }
    }
}
