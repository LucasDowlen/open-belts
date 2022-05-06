using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldLighting : MonoBehaviour
{
    public float loopDuration;
    public Color bright;
    public Color dark;
    //(91, 80, 130)
    public UnityEngine.Rendering.Universal.Light2D light;

    void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }
    void Update()
    {
        float time = Mathf.PingPong(Time.time, loopDuration) / loopDuration;
        light.color = Color.Lerp(bright, dark, time);
    }
}
