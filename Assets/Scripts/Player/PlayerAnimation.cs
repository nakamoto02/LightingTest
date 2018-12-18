using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        Transform    transformCache;
        Animator     animator;
        PlayerMover  pMoveController;
        PlayerCamera pCameraController;

        [SerializeField] Transform rHandDownTransform;
        [SerializeField] Transform rHandUpTransform;
        [SerializeField] Transform rElbowTransform;

        public bool isHand;

        bool isAction = false;
        Vector2 viewAxis;

        float handValue = 0;

        //=================================================
        void Start()
        {
            transformCache    = transform;
            animator          = GetComponent<Animator>();
            pMoveController   = GetComponent<PlayerMover>();
            pCameraController = GetComponent<PlayerCamera>();

            viewAxis = new Vector2(0, 0);
            IPlayerInput pInput = GetComponent<IPlayerInput>();
            pInput.OnActionButton += ChangeActionKeyState;
            pInput.OnMoveAxis     += MoveAction;
            pInput.OnEyesAxis     += EyesAction;
        }
        void FixedUpdate()
        {
            if (isAction) handValue = Mathf.Min(handValue + 2 * Time.deltaTime, 1.0f);
            else handValue = Mathf.Max(handValue - 2 * Time.deltaTime, 0.0f);
        }
        void OnAnimatorIK()
        {
            if (!animator) return;

            if(isHand)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                //animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, Vector3.Lerp(rHandDownTransform.position, rHandUpTransform.position, handValue));
                animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.Lerp(rHandDownTransform.rotation, rHandUpTransform.rotation, handValue));
                //animator.SetIKHintPosition(AvatarIKHint.RightElbow, rElbowTransform.position);
            }
            
        }
        //-------------------------------------------------
        //  イベントに登録するメソッド
        //-------------------------------------------------
        // アクションキー
        void ChangeActionKeyState(bool isPush) { isAction = isPush; }
        // 移動キー
        void MoveAction(Vector2 axis)
        {
            animator.SetBool("Move", axis != Vector2.zero);
        }
        // 視点キー
        void EyesAction(Vector2 axis) { viewAxis = axis; }
        //-------------------------------------------------
        //  アニメーションイベント
        //-------------------------------------------------
        void StartAnimStanding()
        {
            isHand = false;
            pMoveController.IsStop = true;
            pCameraController.Mode = ViewMode.Tracking;
        }
        void EndAnimStanding()
        {
            isHand = true;
            pMoveController.IsStop = false;
            pCameraController.Mode = ViewMode.Manual;
        }
    }
}