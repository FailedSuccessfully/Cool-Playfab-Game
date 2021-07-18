using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Oscilator : MonoBehaviour
{
    public float amplitude = 1f;
    public float rate = 1f;
    public int octaves = 1;
    public bool addNoise = false;
    private float offset = 0f;

    public Vector2 point {
        get{
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        //Oscilate();
    }

    public void Oscilate(){
        float y = 0;
        for (int i = 0; i < octaves; i++)
        {
            y += Mathf.Sin(Time.time * rate * Mathf.Pow(2, i))  * amplitude;
            if (addNoise)
                y *= Mathf.PerlinNoise(Time.time, Time.deltaTime);
        }
        y+= offset;
        point = new Vector2(point.x, y);
    }
}
