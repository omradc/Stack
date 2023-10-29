using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color : MonoBehaviour
{
    public static Color instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        color1 = new Color32((byte)(color1.r + Random.Range(0, 255)), (byte)(color1.g + Random.Range(0, 255)), (byte)(color1.b + Random.Range(0, 255)), color1.a);
        color2 = new Color32((byte)(color2.r + Random.Range(0, 255)), (byte)(color2.g + Random.Range(0, 255)), (byte)(color2.b + Random.Range(0, 255)), color2.a);
        color3 = new Color32((byte)(color3.r + Random.Range(0, 255)), (byte)(color3.g + Random.Range(0, 255)), (byte)(color3.b + Random.Range(0, 255)), color3.a);
        color4 = new Color32((byte)(color4.r + Random.Range(0, 255)), (byte)(color4.g + Random.Range(0, 255)), (byte)(color4.b + Random.Range(0, 255)), color4.a);
        color5 = new Color32((byte)(color5.r + Random.Range(0, 255)), (byte)(color5.g + Random.Range(0, 255)), (byte)(color5.b + Random.Range(0, 255)), color5.a);
        color = color1;
    }

    [Range(0, 1)] public float smoothness;
    public byte colorValue;
    public float colorChangeNumber;
    float number;
    public Color32 color1;
    public Color32 color2;
    public Color32 color3;
    public Color32 color4;
    public Color32 color5;
    public Color32 color;
    void Start()
    {

    }
    public void StackColor(GameObject stack)
    {
        number++;
        if (number > colorChangeNumber)
        {
            number = 0;
            color1 = color2;
            color2 = color3;
            color3 = color4;
            color4 = color5;
            color5 = color;

            color = color1;
        }
        color = new Color32((byte)(color.r + colorValue), (byte)(color.g + colorValue), (byte)(color.b + colorValue), color.a);
        stack.GetComponent<Renderer>().material.color = Color32.Lerp(stack.GetComponent<Renderer>().material.color, color, smoothness);
    }
}
