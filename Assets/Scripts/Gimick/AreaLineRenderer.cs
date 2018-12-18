using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lumiere.Gimick
{
    [RequireComponent(typeof(LineRenderer))]
    public class AreaLineRenderer : MonoBehaviour
    {
        [SerializeField] LayerMask mask;

        LineRenderer lineRenderer;

        List<IDrawLine> drawLineObjects = new List<IDrawLine>();
        Vector3 beforeTopPos;

        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public bool IsDraw { get { return lineRenderer.positionCount > 1; } }
        public int LineObjectCount { get { return lineRenderer.positionCount; } }
        // 先頭
        public IDrawLine TopLineObject { get { return TopNumLineObject(0); } }
        // 二番目
        public IDrawLine SecondLineObject { get { return TopNumLineObject(1); } }
        //=================================================
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        void Update()
        {
            if (IsDraw) DrawLineUpdate();
        }
        //-------------------------------------------------
        //  Public
        //-------------------------------------------------
        // 初期化
        public void InitLineObjects(IDrawLine obj)
        {
            drawLineObjects.Clear();
            InitLineRenderer();
            AddLineObject(obj);
        }

        // 追加
        public void AddLineObject(IDrawLine obj)
        {
            drawLineObjects.Add(obj);
            lineRenderer.positionCount++;

            if (!IsDraw) return;

            beforeTopPos = TopLineObject.DrawPosition;
            DrawLineUpdate();
        }

        // 削除
        public void RemoveLineObject(IDrawLine obj)
        {
            drawLineObjects.Remove(obj);
            lineRenderer.positionCount--;

            if (!IsDraw) return;

            beforeTopPos = TopLineObject.DrawPosition;
            DrawLineUpdate();
        }

        // 線の表示・非表示
        public void LineEnabled(bool value)
        {
            lineRenderer.enabled = value;
        }

        // 線がぶつかったかどうか
        public bool IsHitLine()
        {
            if (IsLineHitCollider()) return true;
            if (IsLineHitLine())     return true;

            return false;
        }
        //-------------------------------------------------
        //  Private
        //-------------------------------------------------
        // 線を描画
        void DrawLineUpdate()
        {
            beforeTopPos = lineRenderer.GetPosition(LineObjectCount - 1);

            for(int i = 0; i < LineObjectCount; ++i)
            {
                lineRenderer.SetPosition(i, drawLineObjects[i].DrawPosition);
            }

            
        }
        // LineRendererの設定を初期化
        void InitLineRenderer()
        {
            if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 0;
        }
        // 線とColliderの当たり判定
        bool IsLineHitCollider()
        {
            float   dis = (SecondLineObject.DrawPosition - TopLineObject.DrawPosition).magnitude;
            Vector3 dir = (SecondLineObject.DrawPosition - TopLineObject.DrawPosition).normalized;
            Ray ray = new Ray(TopLineObject.DrawPosition, dir);

            return Physics.Raycast(ray, dis, mask);
        }
        // 線と線の当たり判定
        bool IsLineHitLine()
        {
            if (LineObjectCount < 3) return false;
            if (beforeTopPos == null) return false;
            if (beforeTopPos == TopLineObject.DrawPosition) return false;

            // プレイヤー、スタンドランプ、1フレーム前のプレイヤーの位置
            Triangle3 triangle = new Triangle3(
                TopLineObject.DrawPosition,
                SecondLineObject.DrawPosition,
                beforeTopPos
                );

            for(int i = 0; i < LineObjectCount - 2; i++)
            {
                Line3 line = new Line3(
                    TopNumLineObject(1 + i).DrawPosition,
                    TopNumLineObject(2 + i).DrawPosition
                    );
                // 三角形と線が平行の時
                if (MyMath.CheckParallelTriangleToLine(triangle, line) || i == 0)
                {
                    Line3 moveLine = new Line3(beforeTopPos, TopLineObject.DrawPosition);
                    // プレイヤーの移動と線が同じ平面にない時
                    if (!MyMath.CheckCrossRayToRay(line, moveLine)) continue;
                    if (MyMath.CheckBack(line, beforeTopPos) || MyMath.CheckBack(line, TopLineObject.DrawPosition)) continue;
                    if (MyMath.CheckForwardRight(line, beforeTopPos) != MyMath.CheckForwardRight(line, TopLineObject.DrawPosition)) return true;

                    continue;
                }
                // 三角形と線が交差している時
                if (MyMath.CheckCrossTriangleToLine(triangle, line, false)) return true;
            }

            return false;
        }

        // 先端からnum番目を取得
        IDrawLine TopNumLineObject(int num)
        {
            return drawLineObjects[(drawLineObjects.Count - 1) - num];
        }
    }
}