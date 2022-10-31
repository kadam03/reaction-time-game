using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTime : MonoBehaviour
{
    public int Hours;
    public int Minutes;
    public int Seconds;
    public int Decimals;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string ToString()
    {
        return string.Format("{0:00}:{1:00}", Minutes, Seconds);
    }
}
