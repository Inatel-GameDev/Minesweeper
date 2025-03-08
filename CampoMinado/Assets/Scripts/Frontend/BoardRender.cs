using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardRender : MonoBehaviour
{
    private Cell[,] grid;

    public Tile tUnknown;
    public Tile tMine;
    public Tile tFlag;
    public Tile tZero;
    public Tile tOne;
    public Tile tTwo;
    public Tile tThree;
    public Tile tFour;
    public Tile tFive;
    public Tile tSix;
    public Tile tSeven;
    public Tile tEight;

    public Tilemap tilemap { get; private set; }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        grid = Global.Instance.grid;
    }

    public void Show()
    {
        tilemap.ClearAllTiles();

        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tilemap.SetTile(grid[i,j].coordinates, ConvertToTile(grid[i,j]));
            }
        }
    }

    private Tile ConvertToTile(Cell cell)
    {
        if (cell.state == State.Default) return tUnknown;
        if (cell.state == State.Flagged) return tFlag;

        if (cell.type == Type.Empty) return tZero;
        if (cell.type == Type.Mine) return tMine;

        switch (cell.num)
        {
            case 1: return tOne;
            case 2: return tTwo;
            case 3: return tThree;
            case 4: return tFour;
            case 5: return tFive;
            case 6: return tSix;
            case 7: return tSeven;
            case 8: return tEight;
        }

        return tUnknown;
    }
}
