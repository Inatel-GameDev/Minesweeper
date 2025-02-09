using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonManager : MonoBehaviour
{
    public static MainButtonManager instance;

    public SpriteRenderer happy;
    public SpriteRenderer won;
    public SpriteRenderer lose;
    public SpriteRenderer press;
    public SpriteRenderer release;

    private void Awake()
    {
        instance = this;

        happy.enabled = false;
        won.enabled = false;
        lose.enabled = false;
        press.enabled = false;
        release.enabled = false;
    }

    private void nMouseDown()
    {
        
    }

    public void enableHappy()
    {
        happy.enabled = true;
        won.enabled = false;
        lose.enabled = false;
        press.enabled = false;
        release.enabled = false;
    }

    public void enableLose()
    {
        happy.enabled = false;
        won.enabled = false;
        lose.enabled = true;
        press.enabled = false;
        release.enabled = false;
    }

    public void enableWon()
    {
        happy.enabled = false;
        won.enabled = true;
        lose.enabled = false;
        press.enabled = false;
        release.enabled = false;
    }

    public void enableRelease()
    {
        happy.enabled = false;
        won.enabled = false;
        lose.enabled = false;
        press.enabled = false;
        release.enabled = true;
    }
}
