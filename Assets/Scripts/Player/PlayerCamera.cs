using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Player
{
    public enum ViewMode
    {
        Manual = 0,
        Tracking
    }

    public class PlayerCamera : MonoBehaviour
    {
        const float ROTATE_SPEED = 180.0f; // 回転速度
        const float DEFAULT_HEIGHT = 1.7f;   // 頭の高さ
        const float SHAKE_FRONT_SPEED = 3.0f;   // 前進する際の揺れる速度
        const float SHAKE_BACK_SPEED = 2.0f;   // 後退する際の揺れる速度
        const float SHAKE_SIDE_SPEED = 2.5f;   // 横移動する際の揺れる速度
        const float SHAKE_WIDTH = 0.05f;  // 揺れ幅

        Camera pCamera;
        Transform cameraTransform;
        [SerializeField] Transform headTransform;

        float value = 0.0f;
        Vector2 moveAxis, viewAxis;

        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public ViewMode Mode { get; set; }
        //=================================================
        void Start()
        {
            pCamera = GetComponentInChildren<Camera>();
            cameraTransform = pCamera.transform;

            moveAxis = viewAxis = new Vector2(0, 0);

            IPlayerInput pInput = GetComponent<IPlayerInput>();
            pInput.OnMoveAxis += MoveAction;
            pInput.OnEyesAxis += EyesAction;
        }
        void FixedUpdate()
        {
            switch (Mode)
            {
                case ViewMode.Manual:   CameraAngle();  ShakeCamera(); break;
                case ViewMode.Tracking: TrackingHead(); break;
            }
        }
        void OnDrawGizmosSelected()
        {
           
            if (headTransform != null)
            {
                Vector3 headUp, headForward, headRight;
                headUp = Quaternion.Euler(headTransform.rotation.eulerAngles) * Vector3.up;
                headForward = Quaternion.Euler(headTransform.rotation.eulerAngles) * Vector3.forward;
                headRight = Quaternion.Euler(headTransform.rotation.eulerAngles) * Vector3.right;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(headTransform.position, headTransform.position + headRight * 3);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(headTransform.position, headTransform.position + headUp * 3);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(headTransform.position, headTransform.position + headForward * 3);
            }
            
        }
        //-------------------------------------------------
        //  イベントに登録するメソッド
        //-------------------------------------------------
        void MoveAction(Vector2 axis) { moveAxis = axis; }
        void EyesAction(Vector2 axis) { viewAxis = axis; }
        //-------------------------------------------------
        //  Private
        //-------------------------------------------------
        // カメラの回転
        void CameraAngle()
        {
            Vector3 angle = cameraTransform.localEulerAngles;
            angle.x += ROTATE_SPEED * viewAxis.y * Time.deltaTime;
            if (angle.x < 0) angle.x += 360;

            if (angle.x < 180) angle.x = Mathf.Min(angle.x, 50);
            else if (angle.x > 180) angle.x = Mathf.Max(angle.x, 300);

            cameraTransform.localEulerAngles = angle;
        }
        // カメラの揺れ
        void ShakeCamera()
        {
            if      (moveAxis.y > 0)   value += Mathf.PI * SHAKE_FRONT_SPEED * Time.deltaTime;
            else if (moveAxis.y < 0)   value += Mathf.PI * SHAKE_BACK_SPEED  * Time.deltaTime;
            else if (moveAxis.x > 0)   value += Mathf.PI * SHAKE_SIDE_SPEED  * Time.deltaTime;
            else if (moveAxis.x < 0)   value += Mathf.PI * SHAKE_SIDE_SPEED  * Time.deltaTime;
            else if (value > Mathf.PI) value =  Mathf.Min(value + Mathf.PI * 3.0f * Time.deltaTime, Mathf.PI * 2);
            else if (value < Mathf.PI) value =  Mathf.Max(value - Mathf.PI * 3.0f * Time.deltaTime, 0);

            if (value > 0) value = Mathf.Repeat(value, Mathf.PI * 2);

            // 頭の高さ + 揺れ
            cameraTransform.localPosition = (Vector3.up * DEFAULT_HEIGHT) + Vector3.up * Mathf.Sin(value) * SHAKE_WIDTH + Vector3.forward * 0.15f;
        }
        // 頭の動きに合わせる
        void TrackingHead()
        {
            Vector3 headForwardDir = Quaternion.Euler(headTransform.rotation.eulerAngles) * Vector3.forward;
            Vector3 headUpDir      = Quaternion.Euler(headTransform.rotation.eulerAngles) * Vector3.up;

            cameraTransform.position = headTransform.position;
            cameraTransform.rotation = Quaternion.LookRotation(headForwardDir, headUpDir);
        }
    }
}


