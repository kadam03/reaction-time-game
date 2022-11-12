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

    // Start is called before the first frame update
    void Start()
    {
        Timer.RemainTime = tileData.Time;
        TileButton.image.color = tileData.TileColor;
        Timer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (tileData.Disappears)
        {
            TextTime.text = string.Format("{0:0.00}", Timer.RemainSecondsWithDecimals);
            Debug.Log(Timer.RemainSecondsWithDecimals);
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

    public void InitTile(TileData td)
    {
        tileData = td;
        Timer.RemainTime = tileData.Time;
        TileButton.image.color = tileData.TileColor;
        Timer.StartTimer();
    }

    IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(5.5f);
    }

    public void OnTileClick()
    {
        GameController.Instance.TileCatched(this.gameObject);
    }
}
