using UnityEngine;

public class Adjust : MonoBehaviour
{
    private void Start()
    {
        AdjustUI();
    }

    public void AdjustUI()
    {
        Camera.main.transform.position = new Vector3(Global.Instance.getWidth() / 2f, Global.Instance.getHeight() / 2f, -10f);
        Camera.main.orthographicSize = Global.Instance.getHeight();
    }
}
