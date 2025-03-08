using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public static Clock Instance;

    public Sprite[] sprites;

    private Image[] uiImages = new Image[3];
    private int time;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        uiImages[0] = transform.Find("U").GetComponent<Image>();
        uiImages[1] = transform.Find("D").GetComponent<Image>();
        uiImages[2] = transform.Find("C").GetComponent<Image>();
    }

    private void Start()
    {
        time = 0;
        Show();
        InvokeRepeating(nameof(IncrementTime), 1f, 1f);
    }

    private void Show()
    {
        int[] indexes = new int[3];
        indexes[0] = time % 10;
        indexes[1] = (time / 10) % 10;
        indexes[2] = (time / 100) % 10;

        for (int i = 0; i < 3; i++)
        {
            if (indexes[i] < sprites.Length)
            {
                uiImages[i].sprite = sprites[indexes[i]];
            }
        }
    }

    public void SetTime(int newTime)
    {
        time = Mathf.Clamp(newTime, 0, 999);
        Show();
    }

    private void IncrementTime()
    {
        if (Global.Instance.gameState != GameState.Running)
        {
            CancelInvoke(nameof(IncrementTime));
        }
        else
        {
            time = Mathf.Clamp(time + 1, 0, 999);
            Show();
        }
    }
}
