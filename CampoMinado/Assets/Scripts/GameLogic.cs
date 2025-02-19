using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance;
    public Decoder decoderFlag;

    public int width = 16;
    public int height = 16;
    public int nBombs = 10;

    private int remainingMines;
    private int remainingTiles;
    private int remainingFlags;

    private bool firstClick = true;
    private bool gameLock = false;
    private Vector2Int firstCoordinates;

    private BoardRender boardRender;
    private Cell[,] grid;

    private void Awake()
    {
        instance = this;

        boardRender = GetComponentInChildren<BoardRender>();
    }

    public void Start()
    {
        remainingMines = nBombs;
        remainingFlags = nBombs;
        remainingTiles = width * height - nBombs;
        gameLock = false;
        firstClick = true;
        MainButtonManager.instance.enableHappy();

        decoderFlag.SetDisplay(remainingFlags);
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
                cell.type = Type.Empty;
                cell.state = State.Default;
                cell.num = 0;
                grid[i, j] = cell;
            }
        }

        SetMines();
    }

    private void ResetGrid()
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
        for (int t = 0; t < nBombs; t++)
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !gameLock) InputProtocol(true);
        if (Input.GetMouseButtonDown(0) && !gameLock) InputProtocol(false);
    }

    private void InputProtocol(bool flag)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = boardRender.tilemap.WorldToCell(worldPosition);

        int x = cellPosition.x;
        int y = cellPosition.y;

        try
        {
            Cell curr = grid[x, y];
            if (flag) Flag(curr);
            else Reveal(curr);
            
            decoderFlag.SetDisplay(remainingFlags);
            CheckVictory();
            boardRender.Show(grid);
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("Você clicou fora da grid");
        }
    }

    private void Flag(Cell curr)
    {
        if (curr == null) return;

        if (curr.state == State.Revealed) return;

        if (curr.state == State.Flagged)
        {
            curr.state = State.Default;
            remainingFlags++;
            remainingMines += (curr.type == Type.Mine) ? 1 : 0;
        }
        else
        {
            curr.state = State.Flagged;
            remainingFlags--;
            remainingMines -= (curr.type == Type.Mine) ? 1 : 0;
        }
    }

    private void Reveal(Cell curr)
    {
        if (curr == null) return;

        while (firstClick && curr.type != Type.Empty)
            ResetGrid();
        firstClick = false;

        if (curr.state != State.Default) return;

        if (curr.type == Type.Mine) GameOverProtocol();

        else
        {
            remainingTiles--;
            curr.state = State.Revealed;

            if (curr.type == Type.Empty) Flood(curr);
        }
    }

    private void Flood(Cell startCell)
    {
        int x = startCell.coordinates.x;
        int y = startCell.coordinates.y;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height && grid[i, j].type != Type.Mine && grid[i, j].state != State.Revealed)
                    Reveal(grid[i, j]);
            }
        }
    }

    private void GameOverProtocol()
    {
        foreach (Cell cell in grid)
            if (cell.type == Type.Mine) cell.state = State.Revealed;

        MainButtonManager.instance.enableLose();
        Clock.instance.Halt();
        gameLock = true;
    }

    private void CheckVictory()
    {
        Debug.Log("Faltam: "+remainingTiles+" Tiles e "+remainingMines+" Flags");
        if (remainingTiles == 0 && remainingMines == 0)
        {
            MainButtonManager.instance.enableWon();
            gameLock = true;
            Clock.instance.Halt();
            Debug.Log("Você venceu!");
        }
    }
}