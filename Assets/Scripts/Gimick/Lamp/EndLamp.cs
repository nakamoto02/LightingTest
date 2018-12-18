using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Gimick.Lamp
{
    public class EndLamp : BaseStageLamp
    {
        const float ACTION_RANGE = 1.5f;

        public delegate void EndEventHandler();
        public event EndEventHandler OneEndConnect;

        bool _isLine;

        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public override bool IsDraw { get { return _isLine; } }
        public override Vector3 DrawPosition { get { return transform.position; } }
        protected override float ActionDistance { get { return ACTION_RANGE; } }
        //-------------------------------------------------
        //  Public
        //-------------------------------------------------
        // 初期化
        public override void Initialize()
        {
            _isLine = true;
        }

        // 線をつないだ際の処理
        public override void ConnectLineAction()
        {
            _isLine = false;
        }
    }
}

