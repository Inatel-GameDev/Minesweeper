using UnityEngine;

public struct Cell
{
    public int num;
    public bool isMine;
    public bool isRevealed;
    public bool isFlagged;

    public Vector3Int coordinates;
}