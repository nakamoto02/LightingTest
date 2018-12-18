using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath
{
    //-----------------------------------------------------
    //  交差判定
    //-----------------------------------------------------
    // 直線 x 直線
    public static bool CheckCrossRayToRay(Line3 line1, Line3 line2)
    {
        Vector3 v3 = line2.Start - line1.Start;

        Vector3 n1 = Vector3.Cross(line1.Length, v3);
        Vector3 n2 = Vector3.Cross(line2.Length, v3);

        return
            n2.magnitude == 0 ||
            n1.magnitude > 0 && Vector3.Cross(n1, n2).magnitude == 0;
    }
    //  三角形 x 線分
    public static bool CheckCrossTriangleToLine(Triangle3 triangle, Line3 line)
    {
        if (CheckTriangleNormalToLine(triangle, line)) return false;

        Vector3 cross = CrossPointTriangleToLine(triangle, line);
        return CheckInsideTriangleToPoint(triangle, cross);
    }
    public static bool CheckCrossTriangleToLine(Triangle3 triangle, Line3 line, bool cull)
    {
        if (cull && CheckTriangleNormalToLine(triangle, line)) return false;

        Vector3 cross = CrossPointTriangleToLine(triangle, line);
        return CheckInsideTriangleToPoint(triangle, cross);
    }
    //-----------------------------------------------------
    //  表面の向きを判定
    //-----------------------------------------------------
    // 三角形 x 線分
    public static bool CheckTriangleNormalToLine(Triangle3 triangle, Line3 line)
    {
        Vector3 tNormal = triangle.Normal;

        Vector3 lhs = line.Start - triangle.P1;
        if (Vector3.Dot(lhs, tNormal) <= 0) return false;

        lhs = line.End - triangle.P1;
        if (Vector3.Dot(lhs, tNormal) >= 0) return false;

        return true;
    }
    //-----------------------------------------------------
    //  左右前後上下判定
    //-----------------------------------------------------
    // 線と点
    public static bool CheckForwardRightUp(Line3 line, Vector3 point)   // 右前上
    {
        return CheckForward(line, point) && CheckRight(line, point) && CheckUp(line, point);
    }
    public static bool CheckForwardRightDown(Line3 line, Vector3 point) // 右前下
    {
        return CheckForward(line, point) && CheckRight(line, point) && CheckDown(line, point);
    }
    public static bool CheckForwardLeftUp(Line3 line, Vector3 point)    // 左前上
    {
        return CheckForward(line, point) && CheckLeft(line, point) && CheckUp(line, point);
    }
    public static bool CheckForwardLeftDown(Line3 line, Vector3 point)  // 左前下
    {
        return CheckForward(line, point) && CheckLeft(line, point) && CheckDown(line, point);
    }
    public static bool CheckBackRightUp(Line3 line, Vector3 point)      // 右後上
    {
        return CheckBack(line, point) && CheckRight(line, point) && CheckUp(line, point);
    }
    public static bool CheckBackRightDown(Line3 line, Vector3 point)    // 右後下
    {
        return CheckBack(line, point) && CheckRight(line, point) && CheckDown(line, point);
    }
    public static bool CheckBackLeftUp(Line3 line, Vector3 point)       // 左後上
    {
        return CheckBack(line, point) && CheckLeft(line, point) && CheckUp(line, point);
    }
    public static bool CheckBackLeftDown(Line3 line, Vector3 point)     // 左後下
    {
        return CheckBack(line, point) && CheckLeft(line, point) && CheckDown(line, point);
    }
    public static bool CheckForwardRight(Line3 line, Vector3 point) // 右前
    {
        return CheckForward(line, point) && CheckRight(line, point);
    }
    public static bool CheckForwardLeft(Line3 line, Vector3 point)  // 左前
    {
        return CheckForward(line, point) && CheckLeft(line, point);
    }
    public static bool CheckForwardUp(Line3 line, Vector3 point)    // 前上
    {
        return CheckForward(line, point) && CheckUp(line, point);
    }
    public static bool CheckForwardDown(Line3 line, Vector3 point)  // 前下
    {
        return CheckForward(line, point) && CheckDown(line, point);
    }
    public static bool CheckBackRight(Line3 line, Vector3 point)    // 右後
    {
        return CheckBack(line, point) && CheckRight(line, point);
    }
    public static bool CheckBackLeft(Line3 line, Vector3 point)     // 左後
    {
        return CheckBack(line, point) && CheckLeft(line, point);
    }
    public static bool CheckBackUp(Line3 line, Vector3 point)       // 後上
    {
        return CheckBack(line, point) && CheckUp(line, point);
    }
    public static bool CheckBackDown(Line3 line, Vector3 point)     // 後下
    {
        return CheckBack(line, point) && CheckDown(line, point);
    }
    public static bool CheckRightUp(Line3 line, Vector3 point)      // 右上
    {
        return CheckRight(line, point) && CheckUp(line, point);
    }
    public static bool CheckRightDown(Line3 line, Vector3 point)    // 右下
    {
        return CheckRight(line, point) && CheckDown(line, point);
    }
    public static bool CheckLeftUp(Line3 line, Vector3 point)       // 左上
    {
        return CheckLeft(line, point) && CheckUp(line, point);
    }
    public static bool CheckLeftDown(Line3 line, Vector3 point)     // 左下
    {
        return CheckLeft(line, point) && CheckDown(line, point);
    }
    public static bool CheckForward(Line3 line, Vector3 point)  // 前
    {
        return Vector3.Dot(point - line.Start, line.Length) >= -Mathf.Epsilon;
    }
    public static bool CheckBack(Line3 line, Vector3 point)     // 後
    {
        return Vector3.Dot(point - line.Start, line.Length) <= Mathf.Epsilon;
    }
    public static bool CheckRight(Line3 line, Vector3 point)    // 右
    {
        return Vector3.Dot(point - line.Start, line.Right) >= -Mathf.Epsilon;
    }
    public static bool CheckLeft(Line3 line, Vector3 point)     // 左
    {
        return Vector3.Dot(point - line.Start, line.Right) <= Mathf.Epsilon;
    }
    public static bool CheckUp(Line3 line, Vector3 point)       // 上
    {
        return Vector3.Dot(point - line.Start, line.Up) >= -Mathf.Epsilon;
    }
    public static bool CheckDown(Line3 line, Vector3 point)     // 下
    {
        return Vector3.Dot(point - line.Start, line.Up) <= Mathf.Epsilon;
    }
    //-----------------------------------------------------
    //  平行判定
    //-----------------------------------------------------
    //  三角形 x 線
    public static bool CheckParallelTriangleToLine(Triangle3 triangle, Line3 line)
    {
        float dot = Vector3.Dot(triangle.Normal, line.Length);
        return -Mathf.Epsilon <= dot && dot <= Mathf.Epsilon;
    }
    //-----------------------------------------------------
    //  交点
    //-----------------------------------------------------
    // 三角形 x 線分
    public static Vector3 CrossPointTriangleToLine(Triangle3 triangle, Line3 line)
    {
        float dots = Vector3.Dot(line.Start - triangle.P1, triangle.Normal);
        float dote = Vector3.Dot(line.End - triangle.P1, triangle.Normal);

        float denom = dots - dote;
        return (dots / denom) * line.Length + line.Start;
    }
    //-----------------------------------------------------
    //  最短距離
    //-----------------------------------------------------
    // 直線 x 点
    public static float NearestLengthRayToPoint(Line3 line, Vector3 point)
    {
        return (NearestPointRayToPoint(line, point) - point).magnitude;
    }
    // 線分 x 点
    public static float NearestLengthLineToPoint(Line3 line, Vector3 point)
    {
        return (NearestPointLineToPoint(line, point) - point).magnitude;
    }
    //-----------------------------------------------------
    //  最近点
    //-----------------------------------------------------
    // 直線 x 点
    public static Vector3 NearestPointRayToPoint(Line3 line, Vector3 point)
    {
        Vector3 lNormal = line.Forward;
        return line.Start + (lNormal * Vector3.Dot(lNormal, point - line.Start));
    }
    // 線分 x 点
    public static Vector3 NearestPointLineToPoint(Line3 line, Vector3 point)
    {
        Vector3 near = NearestPointRayToPoint(line, point);
        // 手前
        float dot = Vector3.Dot(line.Length, near - line.Start);
        if (dot < 0) return line.Start;
        // 奥
        float mag = (near - line.Start).magnitude;
        if (line.Magnitude < mag) return line.End;
        return near;
    }
    //-----------------------------------------------------
    //  内側に点があるかどうか
    //-----------------------------------------------------
    // 三角形
    public static bool CheckInsideTriangleToPoint(Triangle3 triangle, Vector3 point)
    {
        Vector3 tNormal = triangle.Normal;

        return
            Vector3.Dot(Vector3.Cross(triangle.P1 - point, triangle.P2 - triangle.P1), tNormal) > -Mathf.Epsilon &&
            Vector3.Dot(Vector3.Cross(triangle.P2 - point, triangle.P3 - triangle.P2), tNormal) > -Mathf.Epsilon &&
            Vector3.Dot(Vector3.Cross(triangle.P3 - point, triangle.P1 - triangle.P3), tNormal) > -Mathf.Epsilon;
    }
}