using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public static float control = 0f;

    public delegate Vector3 Function(float u, float v, float t);

    public enum FunctionNames
    {
        Wave, Wave2D, MultiWave, MultiWave2D, Ripple,
        Ripple2D, Sphere, Flower, Carambola, Torus, Mixed
    }

    public static Function[] Functions = { Wave, Wave2D, MultiWave,
        MultiWave2D, Ripple, Ripple2D, Sphere, Flower, Carambola, Torus, Mixed};

    public static Function GetFunction(FunctionNames name)
    {
        //int floorControl = (int)Floor(control);
        return Functions[(int)name];
    }
    // 静态类不能继承自 MonoBehaviour
    // 静态类不能声明 Start Update 方法

    // 正弦波
    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + t));
        p.z = v;
        return p;
    }

    public static Vector3 Wave2D(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + t)) * Sin(PI * (v + t)) * 0.5f;
        p.z = v;
        return p;
    }

    // 多正弦波
    public static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        float y = Sin(PI * (u + 0.5f * t));
        y += Sin(2 * PI * (u + t)) * 0.5f;
        //乘法优于除法,且编译器会将常量表达式（例如1f / 2f以及2f * Mathf.PI）简化为单个数字
        p.y = y * (2f / 3f);
        p.z = v;
        return p;
    }

    // 多正弦波
    public static Vector3 MultiWave2D(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        float y = Sin(PI * (u + v + 0.5f * t)) * 4f;
        y += Sin(2 * PI * (v + 2f * t)) * 0.5f;
        y +=Sin(PI * (u + t));
        p.y = y * (2f / 11f);
        p.z = v;
        return p;
    }

    //多重波
    public static Vector3 Ripple(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        float d = Abs(u);
        float y = Sin(4f * PI * (d - t * 0.1f));
        p.y = y / (1f + d * 10f);
        p.z = v;
        return p;
    }

    //多重波2D
    public static Vector3 Ripple2D(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        float d = Sqrt(u * u + v * v);
        float y = Sin(PI * (4f * d - t));
        p.y = y / (1f + d * 10f);
        p.z = v;
        return p;
    }

    // get mixed Y value by slide block
    public static Vector3 Mixed(float u , float v, float t)
    {
        Vector3 p, arg1, arg2;
        int floorControl = (int)Floor(control);
        float conbineRate = control - (float)floorControl;
        float y = 0;
        switch (floorControl)
        {
            case 0:
                arg1 = Wave(u, v, t);
                arg2 = MultiWave(u, v, t);
                y = arg1.y * (1.0f - conbineRate) + arg2.y * conbineRate;
                break;
            case 1:
                arg1 = MultiWave(u, v, t);
                arg2 = Ripple(u, v, t);
                y = arg1.y * (1.0f - conbineRate) + arg2.y * conbineRate;
                break;
            default: // 2
                arg1 = Ripple(u, v, t);
                y = arg1.y;
                break;
        }
        p.x = u;
        p.y = y;
        p.z = v;
        return p;
    }

    //3D Function
    public static Vector3 Sphere(float u , float v, float t){
        float r = 0.5f + 0.5f * Sin(PI * t);
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Flower(float u , float v, float t){
        float r = 0.8f + Sin(PI * (6f * u + t)) * 0.1f;
        r += Sin(PI * (4f * v + t)) * 0.1f;
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Carambola(float u , float v, float t){
        //float r = 0.9f + 0.1f * Sin(PI * (8f * u + t));
        //float r = 0.9f + 0.1f * Sin(PI * (8f * v + t));
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Torus(float u , float v, float t){
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
}
