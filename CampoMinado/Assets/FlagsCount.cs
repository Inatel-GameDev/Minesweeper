using UnityEngine;
using UnityEngine.UI;

public class FlagsCount : MonoBehaviour
{
    public static FlagsCount Instance;

    public Sprite[] sprites;

    private Image[] uiImages = new Image[3];

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
        Show();
    }

    public void Show()
    {
        int[] indexes = new int[3];
        indexes[0] = Global.Instance.rFlags % 10;
        indexes[1] = (Global.Instance.rFlags / 10) % 10;
        indexes[2] = (Global.Instance.rFlags / 100) % 10;

        for (int i = 0; i < 3; i++)
        {
            if (indexes[i] < sprites.Length)
            {
                uiImages[i].sprite = sprites[indexes[i]];
            }
        }
    }
}
