using UnityEngine;

public class Decoder : MonoBehaviour
{
    public GameObject[] unidadeSprites;
    public GameObject[] dezenaSprites;
    public GameObject[] centenaSprites;

    public void SetDisplay(int value)
    {
        value = Mathf.Clamp(value, 0, 999);

        int unidade = value % 10;
        int dezena = (value / 10) % 10;
        int centena = (value / 100) % 10;

        UpdateSegment(unidadeSprites, unidade);
        UpdateSegment(dezenaSprites, dezena);
        UpdateSegment(centenaSprites, centena);
    }

    private void UpdateSegment(GameObject[] segment, int digit)
    {
        for (int i = 0; i < segment.Length; i++)
        {   
            segment[i].SetActive(i == digit);
        }
    }
}
