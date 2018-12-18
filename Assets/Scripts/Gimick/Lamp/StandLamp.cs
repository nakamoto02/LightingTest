using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Gimick.Lamp
{
    public class StandLamp : BaseStageLamp
    {
        const float ACTION_DEFAULT_RANGE = 1.0f;
        const float ACTION_ADD_RANGE     = 1.0f;

        [SerializeField] Transform linePoint;
        [SerializeField] int connectMax;
        [SerializeField] int connectNum;

        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public override bool IsDraw { get { return connectNum < connectMax; } }
        public override Vector3 DrawPosition { get { return (linePoint != null) ? linePoint.position : transform.position; } }
        protected override float ActionDistance { get { return ACTION_DEFAULT_RANGE + ACTION_ADD_RANGE * connectNum; } }
        //-------------------------------------------------
        //  Public
        //-------------------------------------------------
        // 初期化
        public override void Initialize()
        {
            connectNum = 0;
        }

        // 線をつないだ際の処理
        public override void ConnectLineAction()
        {
            connectNum++;
        }
    }
}