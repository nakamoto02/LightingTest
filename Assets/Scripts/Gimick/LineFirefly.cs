using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Gimick
{
    public delegate void ConnectedEventHandler(IDrawLine target);

    public class LineFirefly : MonoBehaviour, IDrawLine
    {
        const float STOP_TIME      = 0.5f;  // 開始時の停止時間
        const float MOVE_TIME      = 3.0f;  // 移動にかける時間
        const float CHECK_DISTANCE = 0.1f;  // 判定距離

        Transform transformCache;
        event ConnectedEventHandler OnTouchTarget;

        float stopTimer, moveValue;

        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public bool IsDraw { get { return true; } }
        public Vector3 DrawPosition { get { return transformCache.position; } }
        public IDrawLine Target { get; private set; }
        //=================================================
        void FixedUpdate()
        {
            if (Target == null) return;

            TrackingTarget();
            CheckDistance();
        }
        //-------------------------------------------------
        //  Public
        //-------------------------------------------------
        public void Initialize(Vector3 position, IDrawLine target, ConnectedEventHandler finishEvent)
        {
            // 表示
            gameObject.SetActive(true);
            // 設定
            if (transformCache == null) transformCache = transform;
            transformCache.position = position;
            Target        = target;
            OnTouchTarget = finishEvent;
            stopTimer = moveValue = 0.0f;
        }
        public void EndProcessing()
        {
            Target = null;
            gameObject.SetActive(false);
        }
        //-------------------------------------------------
        //  Private
        //-------------------------------------------------
        // 追尾
        void TrackingTarget()
        {
            if(IsStop()) moveValue += Time.deltaTime / MOVE_TIME;

            transformCache.position = Vector3.Lerp(transformCache.position, Target.DrawPosition, moveValue);
        }

        // 停止確認
        bool IsStop()
        {
            stopTimer += Time.deltaTime;
            return stopTimer >= STOP_TIME;
        }

        // 距離の確認
        void CheckDistance()
        {
            float dis = (Target.DrawPosition - transformCache.position).magnitude;

            if(dis < CHECK_DISTANCE)
            {
                OnTouchTarget(Target);
                EndProcessing();
            }
        }
    }
}