using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  線分クラス   頂点2つ
//
public class Line2
{
    Vector2[] vertexs = new Vector2[2];

    //-----------------------------------------------------
    //  コンストラクタ
    //-----------------------------------------------------
    public Line2()
    {
        vertexs[0] = vertexs[1] = new Vector2(0, 0);
    }
    public Line2(Vector2 start, Vector2 end)
    {
        vertexs[0] = start; vertexs[1] = end;
    }
    //-----------------------------------------------------
    //  プロパティ
    //-----------------------------------------------------
    public Vector2 Start  { get { return vertexs[0]; } set { vertexs[0] = value; } }
    public Vector2 End    { get { return vertexs[1]; } set { vertexs[1] = value; } }
    public Vector2 Length { get { return End - Start; } }
    public float Magnitude
    {
        get { return Mathf.Pow((Length.x * Length.x) + (Length.y * Length.y), 0.5f); }
    }
    public Vector2 NormalDir { get { return Length / Magnitude; } }
    


    public static implicit operator Line3(Line2 value)
    {
        return new Line3(value.Start, value.End);
    }
}
public class Line3
{
    Vector3[] vertexs = new Vector3[2];

    //-----------------------------------------------------
    //  コンストラクタ
    //-----------------------------------------------------
    public Line3()
    {
        vertexs[0] = vertexs[1] = new Vector3(0, 0, 0);
    }
    public Line3(Vector3 start, Vector3 end)
    {
        vertexs[0] = start; vertexs[1] = end;
    }
    //-----------------------------------------------------
    //  プロパティ
    //-----------------------------------------------------
    public Vector3 Start { get { return vertexs[0]; } set { vertexs[0] = value; } }
    public Vector3 End   { get { return vertexs[1]; } set { vertexs[1] = value; } }
    public Vector3 Length { get { return End - Start; } }
    public float Magnitude
    {
        get { return Mathf.Pow((Length.x * Length.x) + (Length.y * Length.y) + (Length.z * Length.z), 0.5f); }
    }
    public Vector3 Forward { get { return Length / Magnitude; } }
    public Vector3 Back  { get { return Quaternion.Euler(new Vector3(0, 180, 0)) * Length / Magnitude; } }
    public Vector3 Right { get { return Quaternion.Euler(new Vector3(0,  90, 0)) * Length / Magnitude; } }
    public Vector3 Left  { get { return Quaternion.Euler(new Vector3(0, 270, 0)) * Length / Magnitude; } }
    public Vector3 Up    { get { return Quaternion.Euler(new Vector3(270, 0, 0)) * Length / Magnitude; } }
    public Vector3 Down  { get { return Quaternion.Euler(new Vector3( 90, 0, 0)) * Length / Magnitude; } }


    public static Line3 Reverse(Line3 line)
    {
        return new Line3(line.End, line.Start);
    }
    public static implicit operator Line2(Line3 value)
    {
        return new Line2(value.Start, value.End);
    }
}