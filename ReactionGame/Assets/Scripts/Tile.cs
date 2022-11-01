using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    //public RTimer Timer = null;
    public TMP_Text TextTime = null;
    public RTimer Timer = null;
    public Button TileButton = null;

    // Start is called before the first frame update
    void Start()
    {
        Timer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        TextTime.text = Timer.Seconds.ToString();
    }

    IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(5.5f);
    }

    public void OnTileClick()
    {
        GameController.Instance.TileCatched();
    }
}
