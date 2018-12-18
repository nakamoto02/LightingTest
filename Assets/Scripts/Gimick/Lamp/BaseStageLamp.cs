using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Gimick.Lamp
{
    public abstract class BaseStageLamp : MonoBehaviour, IDrawLine
    {
        //=================================================
        void Start()
        {
            Initialize();
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(DrawPosition, ActionDistance);
        }
        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        // 線を繋げれる状態か
        public abstract bool IsDraw { get; }

        // 線を繋げる位置
        public abstract Vector3 DrawPosition { get; }

        // 行動を起こす範囲
        protected abstract float ActionDistance { get; }

        //-------------------------------------------------
        //  抽象メソッド
        //-------------------------------------------------
        // 初期化
        public abstract void Initialize();

        // 線をつないだ際の処理
        public abstract void ConnectLineAction();

        //-------------------------------------------------
        //  Public
        //-------------------------------------------------
        // 行動を起こす範囲内かどうか
        public bool IsActionInRange(Vector3 point)
        {
            float dis = (DrawPosition - point).magnitude;
            return dis < ActionDistance;
        }
    }
}