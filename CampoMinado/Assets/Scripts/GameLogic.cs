using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int nBombs = 10;

    private BoardRender boardRender;
    private Cell[,] grid;

    private void Awake()
    {
        boardRender = GetComponentInChildren<BoardRender>();
    }

    private void Start()
    {
        SetGrid();
        boardRender.Show(grid);
    }

    private void SetGrid()
    {
        grid = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell cell = new Cell();
                cell.coordinates = new Vector3Int(i, j, 0);
                cell.isMine = false;
                cell.isFlagged = false;
                cell.isRevealed = true;
                cell.num = 0;
                grid[i, j] = cell;
            }
        }

        SetMines();
    }

    private void SetMines()
    {
        for (int t = 0; t < nBombs; t++)
        {
            int i, j;
            do
            {
                i = Random.Range(0, width);
                j = Random.Range(0, height);
            } while (grid[i, j].isMine);

            grid[i, j].isMine = true;
            SetNumbers(i, j);
        }
    }

    private void SetNumbers(int i, int j)
    {
        for (int ii = i - 1; ii <= i + 1; ii++)
        {
            for (int jj = j - 1; jj <= j + 1; jj++)
            {
                if (ii >= 0 && ii < width && jj >= 0 && jj < height && !grid[ii, jj].isMine)
                {
                    grid[ii, jj].num++;
                }
            }
        }
    }
}
