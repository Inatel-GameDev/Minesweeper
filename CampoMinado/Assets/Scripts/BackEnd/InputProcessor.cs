using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InputProcessor : MonoBehaviour
{
    private BoardRender boardRender;
    private GameLogic gameLogic;

    private int width;
    private int height;
    private Cell[,] grid;

    private void Awake()
    {
        gameLogic = GetComponent<GameLogic>();
        boardRender = GetComponentInChildren<BoardRender>();
    }

    public void Start()
    {
        width = Global.Instance.getWidth();
        height = Global.Instance.getHeight();
        grid = Global.Instance.grid;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && Global.Instance.gameState == GameState.Running) InputProtocol(true);
        if (Input.GetMouseButtonDown(0) && Global.Instance.gameState == GameState.Running) InputProtocol(false);
    }

    private void InputProtocol(bool state)
    {
        ProcessInput(state);
        boardRender.Show();
        CheckVictory();
        MainButton.Instance.CheckGameState();
    }

    private void ProcessInput(bool state)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = boardRender.tilemap.WorldToCell(worldPosition);

        int x = cellPosition.x;
        int y = cellPosition.y;

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            Cell curr = grid[x,y];
            if (state) ToggleFlag(curr);
            else Reveal(curr);
        }
    }

    private void ToggleFlag(Cell curr)
    {
        if (curr.state == State.Revealed) return;

        if (curr.state == State.Flagged)
        {
            curr.state = State.Default;
            Global.Instance.changeRFlags(1);

            if (curr.type == Type.Mine) Global.Instance.changeRBombs(1);
        }
        else if (Global.Instance.rFlags > 0)
        {
            curr.state = State.Flagged;
            Global.Instance.changeRFlags(-1);
            if (curr.type == Type.Mine) Global.Instance.changeRBombs(-1);
        }
    }

    private void Reveal(Cell curr)
    {
        while (Global.Instance.firstClick && curr.type != Type.Empty) gameLogic.ResetGrid();

        Global.Instance.firstClick = false;

        if (curr.state != State.Default) return;

        if (curr.type == Type.Mine) GameOverProtocol();

        else
        {
            Global.Instance.changeRTiles(-1);
            curr.state = State.Revealed;

            if (curr.type == Type.Empty) Flood(curr);
        }
    }

    private void Flood(Cell curr)
    {
        int x = curr.coordinates.x;
        int y = curr.coordinates.y;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height && grid[i, j].type != Type.Mine && grid[i, j].state != State.Revealed)
                {
                    if (grid[i, j].state == State.Flagged) ToggleFlag(grid[i, j]);
                    Reveal(grid[i, j]);
                }
            }
        }
    }

    private void GameOverProtocol()
    {
        foreach (Cell cell in grid)
            if (cell.type == Type.Mine) cell.state = State.Revealed;

        Global.Instance.gameState = GameState.Lost;
    }

    private void CheckVictory()
    {
        Debug.Log("Faltam: " + Global.Instance.rTiles + " Tiles e " + Global.Instance.rBombs + " Flags");
        if (Global.Instance.rBombs == 0 && Global.Instance.rBombs == 0)
            Global.Instance.gameState = GameState.Won;
    }
}
