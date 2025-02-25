using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adjust : MonoBehaviour
{
    public static Adjust Instance;
    public Camera camera; // Alterado de GameObject para Camera

    private int width;
    private int height;
    private float Yoffset;

    private void Awake()
    {
        Instance = this;

        // Se a câmera não foi atribuída no Inspector, tenta pegar a câmera principal
        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    private void Start()
    {
        AdjustUI();
    }

    public void AdjustUI()
    {
        width = Global.Instance.getWidth();
        height = Global.Instance.getHeight();
        Yoffset = height - 1;

        if (camera != null)
        {
            camera.transform.position = new Vector3(width / 2f, height / 2, -10f);
            camera.orthographicSize = height; // Ajusta o tamanho da câmera conforme a altura
        }
        else
        {
            Debug.LogError("A câmera não foi atribuída!");
        }

        transform.position = new Vector3(width / 2f, height / 2 + Yoffset, 0f);
    }
}
