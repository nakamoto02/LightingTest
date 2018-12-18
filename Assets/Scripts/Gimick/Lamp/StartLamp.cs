using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Gimick.Lamp
{
    public class StartLamp : BaseStageLamp
    {
        const float ACTION_RANGE = 1.5f;

        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public override bool IsDraw { get { return true; } }
        public override Vector3 DrawPosition { get { return transform.position; } }
        protected override float ActionDistance { get { return ACTION_RANGE; } }
        //-------------------------------------------------
        //  Public
        //-------------------------------------------------
        // 初期化
        public override void Initialize()
        {
            
        }

        // 線をつないだ際の処理
        public override void ConnectLineAction()
        {
            
        }
    }
}