using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    public static MainButton Instance;

    private Image currImage;
    public Sprite[] images;

    private void Awake()
    {
        Instance = this;

        currImage = GetComponent<Image>();
        currImage.sprite = images[0];
    }

    public void CheckGameState()
    {
        if (Global.Instance.gameState == GameState.Won) currImage.sprite = images[4];
        if (Global.Instance.gameState == GameState.Lost) currImage.sprite = images[3];
    }

    public void ReloadScene()
    {
        if (Global.Instance.firstClick) DataSceneBuffer.Instance.ChangeDiff();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
