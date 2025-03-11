using UnityEngine;

public class DataSceneBuffer : MonoBehaviour
{
    public static DataSceneBuffer Instance;
    public int diff = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeDiff()
    {
        diff = (diff == 2) ? 0 : diff + 1;
    }
}
