using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpController : MonoBehaviour
{
    public Canvas HelpCanvas = null;
    public GameObject DrawArea = null;
    public GameObject SampleText = null;
    public List<TileData> TileTypes = new List<TileData>();

    float leftOffset = 120;
    float topOffset = -60;

    // Start is called before the first frame update
    void Start()
    {
        ListTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickBack()
    {
        SceneManager.LoadScene(0);
    }

    void ListTiles()
    {
        foreach (TileData td in TileTypes)
        {
            CreateTileImage(td.TileColor, td.Description);
            topOffset -= 160;
        }
    }

    void CreateTileImage(Color col, string desc)
    {
        GameObject tileImg = new GameObject();
        tileImg.AddComponent<Image>();
        tileImg.GetComponent<Image>().color = col;
        tileImg.transform.SetParent(HelpCanvas.transform);
        tileImg.transform.localScale = new Vector3(1, 1, 1);

        RectTransform rt = DrawArea.GetComponent<RectTransform>();
        float areaX = (rt.rect.xMin + leftOffset) * HelpCanvas.transform.localScale.x;
        float areaY = (rt.rect.yMax + topOffset) * HelpCanvas.transform.localScale.y;

        tileImg.transform.position = new Vector3(areaX, areaY, 1) + DrawArea.transform.position;
        CreateTileDescription(desc, tileImg.transform.position);

    }

    void CreateTileDescription(string desc, Vector3 refPos)
    {
        TMP_Text description = Instantiate(SampleText, new Vector3(140, 0) + refPos, Quaternion.identity).GetComponent<TMP_Text>();
        description.text = desc;
        description.gameObject.SetActive(true);
        description.transform.SetParent(HelpCanvas.transform);
        description.transform.localScale = new Vector3(1, 1, 1);
    }
}
