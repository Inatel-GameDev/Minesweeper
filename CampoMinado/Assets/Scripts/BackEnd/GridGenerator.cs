using System;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private BoardRender boardRender;

    private int width;
    private int height;
    private int nbombs;

    private Cell[,] grid;

    private void Awake()
    {
        boardRender = GetComponentInChildren<BoardRender>();
    }

    public void Start()
    {
        width = Global.Instance.getWidth();
        height = Global.Instance.getHeight();
        nbombs = Global.Instance.getNBombs();

        grid = Global.Instance.grid;

        SetGrid();
        boardRender.Show();
    }

    private void SetGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = new Cell();
                cell.coordinates = new Vector3Int(i, j, 0);
                cell.type = Type.Empty;
                cell.state = State.Default;
                cell.num = 0;
                grid[i, j] = cell;
            }
        }

        SetMines();
    }

    public void ResetGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j].num = 0;
                grid[i, j].type = Type.Empty;
            }
        }

        SetMines();
    }

    private void SetMines()
    {
        for (int t = 0; t < nbombs; t++)
        {
            int i, j;
            do
            {
                i = UnityEngine.Random.Range(0, width);
                j = UnityEngine.Random.Range(0, height);
            } while (grid[i, j].type == Type.Mine);

            grid[i, j].type = Type.Mine;
            SetNumbers(i, j);
        }
    }

    private void SetNumbers(int i, int j)
    {
        for (int ii = i - 1; ii <= i + 1; ii++)
        {
            for (int jj = j - 1; jj <= j + 1; jj++)
            {
                if (ii >= 0 && ii < width && jj >= 0 && jj < height && grid[ii, jj].type != Type.Mine)
                {
                    grid[ii, jj].type = Type.Number;
                    grid[ii, jj].num++;
                }
            }
        }
    }
}