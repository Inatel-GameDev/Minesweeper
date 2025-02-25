using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global Instance;

    public int dif;
    private int[] width = { 10, 18, 24 };
    private int[] height = { 8, 15, 20 };
    private int[] nBombs = { 10, 40, 99 };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dif = 0;
    }

    public void changeDif()
    {
        dif = (dif < 2) ? dif + 1 : 0;
    }

    public int getWidth()
    {
        return width[dif];
    }

    public int getHeight()
    {
        return height[dif];
    }

    public int getNBombs()
    {
        return nBombs[dif];
    }
}
