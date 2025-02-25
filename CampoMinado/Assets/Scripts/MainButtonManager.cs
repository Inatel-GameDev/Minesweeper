using System.Globalization;
using UnityEngine;

public class MainButtonManager : MonoBehaviour
{
    public static MainButtonManager instance;
    public Clock clock;

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

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameLogic.instance.getFirstClick())
            {
                Global.Instance.changeDif();
                Adjust.Instance.AdjustUI();
                GameLogic.instance.Start();
                clock.Start();
            }
            else
            {
                GameLogic.instance.Start();
                clock.Start();
            }
        }
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
