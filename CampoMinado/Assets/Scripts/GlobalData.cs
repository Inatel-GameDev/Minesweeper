using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global Instance;
    private DataSceneBuffer buffer;

    public int[] widths = { 10, 18, 24 };
    public int[] heights = { 8, 14, 20 };
    public int[] nbombs = { 10, 40, 99 };

    public int flags;
    public int rTiles;
    public int rFlags;
    public int rBombs;
    
    public GameState gameState;
    public int diff;

    public bool firstClick;

    public Cell[,] grid;

    public void Awake()
    {
        Instance = this;
        
        buffer = FindObjectOfType<DataSceneBuffer>();
        diff = buffer.diff;
        gameState = GameState.Running;
        firstClick = true;
        flags = nbombs[diff];
        rFlags = nbombs[diff];
        rBombs = nbombs[diff];
        rTiles = widths[diff] * heights[diff] - rBombs;

        grid = new Cell[widths[diff], heights[diff]];
    }

    public int getWidth()
    {
        return widths[diff];
    }

    public int getHeight()
    {
        return heights[diff];
    }
    
    public int getNBombs()
    {
        return nbombs[diff];
    }

    public void disableFirstClick()
    {
        firstClick = false;
    }

    public void changeRFlags(int value)
    {
        rFlags += value;
    }

    public void changeRBombs(int value)
    {
        rBombs += value;
    }

    public void changeRTiles(int value)
    {
        rTiles += value;
    }
}
