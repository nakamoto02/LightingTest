using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  三角形クラス  頂点3つ
//
public class Triangle2
{
    Vector2[] vertexs = new Vector2[3];

    //-----------------------------------------------------
    //  コンストラクタ
    //-----------------------------------------------------
    public Triangle2()
    {
        vertexs[0] = vertexs[1] = vertexs[2] = new Vector2(0, 0);
    }
    public Triangle2(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        vertexs[0] = p1; vertexs[1] = p2; vertexs[2] = p3;
    }
    //-----------------------------------------------------
    //  プロパティ
    //-----------------------------------------------------
    public Vector2 P1 { get { return vertexs[0]; } set { vertexs[0] = value; } }
    public Vector2 P2 { get { return vertexs[1]; } set { vertexs[1] = value; } }
    public Vector2 P3 { get { return vertexs[2]; } set { vertexs[2] = value; } }


    //public static implicit operator Triangle3(Triangle2 value)
    //{
    //    return new Triangle3(value.P1, value.P2, value.P3);
    //}
}
public class Triangle3
{
    Vector3[] vertexs = new Vector3[3];

    //-----------------------------------------------------
    //  コンストラクタ
    //-----------------------------------------------------
    public Triangle3()
    {
        vertexs[0] = vertexs[1] = vertexs[2] = new Vector3(0, 0, 0);
    }
    public Triangle3(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        vertexs[0] = p1; vertexs[1] = p2; vertexs[2] = p3;
    }
    //-----------------------------------------------------
    //  プロパティ
    //-----------------------------------------------------
    public Vector3 P1 { get { return vertexs[0]; } set { vertexs[0] = value; } }
    public Vector3 P2 { get { return vertexs[1]; } set { vertexs[1] = value; } }
    public Vector3 P3 { get { return vertexs[2]; } set { vertexs[2] = value; } }
    public Vector3 Normal
    {
        get { return Vector3.Cross(P2 - P1, P3 - P1).normalized; }
    }


    //public static implicit operator Triangle2(Triangle3 value)
    //{
    //    return new Triangle2(value.P1, value.P2, value.P3);
    //}
}