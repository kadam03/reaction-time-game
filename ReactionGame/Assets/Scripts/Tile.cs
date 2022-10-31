using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public RTimer Timer = null;
    public TMP_Text TextTime = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndStart());
    }

    // Update is called once per frame
    void Update()
    {
        TextTime.text = Timer.Minutes + ":" + Timer.Seconds;
    }

    IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(5.5f);
        Timer.StartTimer();
    }
}
