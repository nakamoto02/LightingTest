using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Player
{
    public class PlayerMover : MonoBehaviour
    {
        const float MOVE_SPEED   = 3.0f;   // 移動速度
        const float ROTATE_SPEED = 180.0f;  // 回転速度

        Transform           transformCache;
        CharacterController controller;
        
        bool  isAction = false;

        Vector2 moveAxis;
        Vector2 viewAxis;
        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public bool IsStop { get; set; }
        //=================================================
        void Start()
        {
            transformCache  = transform;
            controller      = GetComponent<CharacterController>();

            moveAxis = viewAxis = new Vector2(0, 0);

            IPlayerInput pInput = GetComponent<IPlayerInput>();
            pInput.OnActionButton += ChangeActionKeyState;
            pInput.OnMoveAxis     += MoveAction;
            pInput.OnEyesAxis     += EyesAction;
        }
        void FixedUpdate()
        {
            if (IsStop) return;

            //Rotation();
            Move();
        }
        //-------------------------------------------------
        //  イベントに登録するメソッド
        //-------------------------------------------------
        // アクションキー
        void ChangeActionKeyState(bool isPush) { isAction = isPush; }
        // 移動キー
        void MoveAction(Vector2 axis) { moveAxis = axis; }
        // 視点キー
        void EyesAction(Vector2 axis) { viewAxis = axis; }
        //-------------------------------------------------
        //  行動
        //-------------------------------------------------
        // 回転
        void Rotation()
        {
            transformCache.Rotate(Vector3.up, ROTATE_SPEED * viewAxis.x * Time.deltaTime);
        }

        // 移動
        void Move()
        {
            // 進行方向
            Vector3 moveDir = transformCache.forward * moveAxis.y + transformCache.right * moveAxis.x;
            if(moveDir.magnitude != 0) moveDir /= moveDir.magnitude;
            // 移動量
            float speed = MOVE_SPEED * ((isAction) ? 0.5f : 1.0f);

            controller.SimpleMove(moveDir * speed);
        }
    }
}