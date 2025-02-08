using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonManager : MonoBehaviour
{
    public SpriteRenderer happy;
    public SpriteRenderer won;
    public SpriteRenderer lose;

    private void Awake()
    {
        happy.enabled = false;
        won.enabled = false;
        lose.enabled = false;
    }

    public void enableHappy()
    {
        happy.enabled = true;
        won.enabled = false;
        lose.enabled = false;
    }

    public void enableLose()
    {
        happy.enabled = false;
        won.enabled = false;
        lose.enabled = true;
    }

    public void enableWon()
    {
        happy.enabled = true;
        won.enabled = true;
        lose.enabled = false;
    }
}
