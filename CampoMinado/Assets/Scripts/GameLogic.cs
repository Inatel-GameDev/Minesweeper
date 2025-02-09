using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance;

    public int width = 16;
    public int height = 16;
    public int nBombs = 10;

    private int remainingFlags;
    private int remainingTiles;

    private bool firstClick = true;
    private Vector2Int firstCoordinates;

    private BoardRender boardRender;
    private Cell[,] grid;

    private void Awake()
    {
        instance = this;

        boardRender = GetComponentInChildren<BoardRender>();
        remainingFlags = nBombs;
        remainingTiles = width * height - nBombs;
    }

    private void Start()
    {
        MainButtonManager.instance.enableHappy();
        SetGrid();
        boardRender.Show(grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) Flag();
        if (Input.GetMouseButtonDown(0)) Reveal();
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
                cell.isRevealed = false;
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
                i = UnityEngine.Random.Range(0, width);
                j = UnityEngine.Random.Range(0, height);
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

    private void Flag()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = boardRender.tilemap.WorldToCell(worldPosition);

        try
        {
            if (!grid[cellPosition.x, cellPosition.y].isRevealed)
            {
                if (grid[cellPosition.x, cellPosition.y].isMine)
                {
                    if (grid[cellPosition.x, cellPosition.y].isFlagged) remainingFlags++;
                    else remainingFlags--;
                }

                grid[cellPosition.x, cellPosition.y].isFlagged = !grid[cellPosition.x, cellPosition.y].isFlagged;

                CheckVictory();
                boardRender.Show(grid);
            }
        } catch (IndexOutOfRangeException)
        {
            Debug.LogError("Você clicou fora da grid");
        }
    }

    private void Reveal()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = boardRender.tilemap.WorldToCell(worldPosition);

        try
        {
            int x = cellPosition.x;
            int y = cellPosition.y;

            if (firstClick)
            {
                do
                {
                    SetGrid();
                } while (grid[x, y].isMine);
                firstClick = false;
            }

            if (grid[x, y].isFlagged || grid[x, y].isRevealed) return;

            if (grid[x, y].isMine)
            {
                Debug.Log("Você perdeu");
                GameOverProtocol();
            }

            else if (grid[x, y].num > 0)
            {
                grid[x, y].isRevealed = true;
                remainingTiles--;
            }

            else if (grid[x, y].num == 0)
            {
                Flood(grid[x, y]);
            }

            CheckVictory();
            boardRender.Show(grid);
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError("Você clicou fora da grid");
        }
    }

    private void Flood(Cell startCell)
    {
        Queue<Cell> queue = new Queue<Cell>();
        queue.Enqueue(startCell);

        while (queue.Count > 0)
        {
            Cell cell = queue.Dequeue();
            int x = cell.coordinates.x;
            int y = cell.coordinates.y;

            if (grid[x, y].isRevealed) continue;

            grid[x, y].isRevealed = true;
            remainingTiles--;

            if (grid[x, y].num > 0) continue;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        if (!grid[nx, ny].isRevealed && !grid[nx, ny].isMine && !grid[nx, ny].isFlagged)
                        {
                            queue.Enqueue(grid[nx, ny]);
                        }
                    }
                }
            }
        }
    }

    private void GameOverProtocol()
    {
        MainButtonManager.instance.enableLose();
        for (int i =  0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i, j].isMine)
                {
                    grid[i, j].isRevealed = true;
                    boardRender.Show(grid);
                }
            }
        }
    }

    private void CheckVictory()
    {
        Debug.Log("Faltam: "+remainingTiles+" Tiles e "+remainingFlags+" Flags");
        if (remainingTiles == 0 && remainingFlags == 0)
        {
            MainButtonManager.instance.enableWon();
            Debug.Log("Você venceu!");
        }
    }
}