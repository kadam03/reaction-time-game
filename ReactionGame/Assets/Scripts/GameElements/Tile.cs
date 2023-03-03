using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileData tileData = null;
    public TMP_Text TextTime = null;
    public RTimer Timer = null;
    public Button TileButton = null;
    public Image Background = null;

    // Start is called before the first frame update
    void Start()
    {
        Timer.RemainTime = tileData.Time;
        Background.color = tileData.TileColor;
        Timer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (tileData.Disappears)
        {
            TextTime.text = string.Format("{0:0.00}", Timer.RemainSecondsWithDecimals);
            if (Timer.RemainTime < 0)
            {
                GameController.Instance.TileDisappeared(this.gameObject);
            }
        }
        else
        {
            TextTime.text = string.Format("{0:0.00}", Timer.SecondsWithDecimals);
        }
    }

    IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(5.5f);
    }

    public void OnTileClick()
    {
        if (tileData.BreakEffect != null && !ProgressController.Instance.ProgData.MutedGame)
        {
            AudioSource.PlayClipAtPoint(tileData.BreakEffect, new Vector3(0,0,-10));
        }
        GameController.Instance.TileCatched(this.gameObject);
    }
}
