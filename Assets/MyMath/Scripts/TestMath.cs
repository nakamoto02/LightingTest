using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMath : MonoBehaviour
{
    Triangle3 triangle = new Triangle3(
        new Vector3(  0, 20, 0),
        new Vector3(-10,  5, 0),
        new Vector3( 10,  5, 0)
        );

    Line3 line1 = new Line3(
        new Vector3(0, 0,   0),
        new Vector3(0, 0,  10)
        );

    Line3 line2 = new Line3(
        new Vector3(-5, 0, 1),
        new Vector3( 5, 15, 0)
        );

    public Vector3 point;

	void Start ()
    {
        Debug.Log(MyMath.CheckRight(line1, point));
        Debug.Log(MyMath.CheckLeft(line1, point));
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(line1.Start, 0.5f);

        Gizmos.color = Color.blue;
        Vector3 dir = line1.Forward;
        Gizmos.DrawLine(line1.Start - dir * line1.Magnitude, line1.Start + dir * line1.Magnitude);

        Gizmos.color = Color.red;
        dir = Quaternion.Euler(new Vector3(0, 90, 0)) * dir;
        Gizmos.DrawLine(line1.Start - dir * line1.Magnitude, line1.Start + dir * line1.Magnitude);

        Gizmos.color = Color.green;
        dir = Quaternion.Euler(new Vector3(0, 0, 90)) * dir;
        Gizmos.DrawLine(line1.Start - dir * line1.Magnitude, line1.Start + dir * line1.Magnitude);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(point, 0.5f);

        Gizmos.color = Color.red;
        dir = line1.Forward;
        if (MyMath.CheckForwardRight(line1, point))
            Gizmos.DrawWireCube(
                line1.Start + line1.Right * line1.Magnitude * 0.5f,
                new Vector3(1, 2, 1) * line1.Magnitude);
        else if (MyMath.CheckBackRight(line1, point))
            Gizmos.DrawWireCube(
                line1.Start + (Quaternion.Euler(new Vector3(0, 135, 0)) * dir) * line1.Magnitude * 0.5f,
                new Vector3(1, 2, 1) * line1.Magnitude);
        else if (MyMath.CheckBackLeft(line1, point))
            Gizmos.DrawWireCube(
                line1.Start + (Quaternion.Euler(new Vector3(0, 225, 0)) * dir) * line1.Magnitude * 0.5f,
                new Vector3(1, 2, 1) * line1.Magnitude);
        else if (MyMath.CheckForwardLeft(line1, point))
            Gizmos.DrawWireCube(
                line1.Start + (Quaternion.Euler(new Vector3(0, 315, 0)) * dir) * line1.Magnitude * 0.5f,
                new Vector3(1, 2, 1) * line1.Magnitude);


    }
}
