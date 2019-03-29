using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class GraphWindow : MonoBehaviour {

 

    public Analyser analyser;
   // public TestData testData;

    VectorLine window;
    List<Vector2> windowCoords;
    public bool drawWindow;

    List<Vector2> graphCoords;
    VectorLine signal;

    public Vector2 pos;
    public Vector2 size;

    public float lineSize;
    public Texture lineMaterial;
    public Color lineColor;
    public Color windowColor;
    private float multiplier;


    void Start () {

        graphCoords = new List<Vector2>();


       
            multiplier = size.x / analyser.breathLowPass.Length;
            for (int i = 0; i < analyser.breathLowPass.Length; i++)
            {
                graphCoords.Insert(i * 2, new Vector2(((i + 1) * multiplier) + pos.x, pos.y));
                graphCoords.Insert(i * 2 + 1, new Vector2(((i + 1) * multiplier) + pos.x, pos.y));
            }

        signal = new VectorLine("GraphLine", graphCoords, lineMaterial, lineSize);
        signal.SetColor(lineColor);

        if (drawWindow)
        {
            windowCoords = new List<Vector2>();
            window = new VectorLine("WindowLine", windowCoords, lineMaterial, lineSize);

            windowCoords.Insert(0, new Vector3(pos.x, pos.y));
            windowCoords.Insert(1, new Vector3(pos.x + size.x, pos.y));

            windowCoords.Insert(2, new Vector3(pos.x + size.x, pos.y));
            windowCoords.Insert(3, new Vector3(pos.x + size.x, pos.y + size.y, 0));

            windowCoords.Insert(4, new Vector3(pos.x + size.x, pos.y + size.y, 0));
            windowCoords.Insert(5, new Vector3(pos.x, pos.y + size.y, 0));

            windowCoords.Insert(6, new Vector3(pos.x, pos.y + size.y, 0));
            windowCoords.Insert(7, new Vector3(pos.x, pos.y, 0));
            window.SetColor(windowColor);
            window.Draw();
        }


    }

    public void setWindowCoords(Vector2 _size, Vector2 _pos)
    {
        size = _size;
        pos = _pos;
        multiplier = size.x / analyser.breathLowPass.Length;

    }

    // Update is called once per frame
    void Update() {


        float minIbi = Helpers.minValueRange(analyser.breathLowPass, analyser.breathLowPass.Length);
        float maxIbi = Helpers.maxValueRange(analyser.breathLowPass, analyser.breathLowPass.Length);

        for (int i = 0; i < analyser.breathLowPass.Length-1; i++)
        {
            if (analyser.breathLowPass[i] > 0)
            {


                Vector2 calcVec2 = new Vector2((i * multiplier) + pos.x, Helpers.scale(minIbi, maxIbi, 1, 0, analyser.breathLowPass[i]) * size.y + pos.y);

               // graphCoords[i * 2] = Vector2.Lerp(graphCoords[i * 2], calcVec2, Time.deltaTime * lerper);
                graphCoords[i * 2] = calcVec2;
                calcVec2 = new Vector2(((i + 1) * multiplier) + pos.x, Helpers.scale(minIbi, maxIbi, 1, 0, analyser.breathLowPass[i + 1]) * size.y + pos.y);
                //graphCoords[i * 2 + 1] = Vector2.Lerp(graphCoords[i * 2 + 1], calcVec2, Time.deltaTime * lerper);
                graphCoords[i * 2 + 1] = calcVec2;
            }
        }

        signal.Draw();
    }

    public void updatePosSize(Vector2 _pos, Vector2 _size)
    {
      pos = _pos;
        size = _size;
        windowCoords = new List<Vector2>();
        window = new VectorLine("WindowLine", windowCoords, lineMaterial, lineSize);
       

        windowCoords.Insert(0, new Vector3(pos.x, pos.y));
        windowCoords.Insert(1, new Vector3(pos.x + size.x, pos.y));

        windowCoords.Insert(2, new Vector3(pos.x + size.x, pos.y));
        windowCoords.Insert(3, new Vector3(pos.x + size.x, pos.y + size.y, 0));

        windowCoords.Insert(4, new Vector3(pos.x + size.x, pos.y + size.y, 0));
        windowCoords.Insert(5, new Vector3(pos.x, pos.y + size.y, 0));

        windowCoords.Insert(6, new Vector3(pos.x, pos.y + size.y, 0));
        windowCoords.Insert(7, new Vector3(pos.x, pos.y, 0));
        window.SetColor(windowColor);
        window.Draw();
    }
}
