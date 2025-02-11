using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private Decoder decoderClock;
    private int time = 0;

    private void Awake()
    {
        decoderClock = GetComponent<Decoder>();
    }

    public void Start()
    {
        time = 0;
        decoderClock.SetDisplay(time);
        CancelInvoke("UseClock");
        InvokeRepeating("UseClock", 1f, 1f);
    }

    private void UseClock()
    {
        if (decoderClock != null)
        {
            time = (time >= 999) ? 0 : time + 1;
            decoderClock.SetDisplay(time);
        }
    }
}
