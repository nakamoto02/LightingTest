using UnityEngine;

namespace Lumiere.Player
{
    public class ManualInput : MonoBehaviour, IPlayerInput
    {
        // イベント
        public event ButtonEventHandler OnActionButton;
        public event AxisEventHandler   OnMoveAxis;
        public event AxisEventHandler   OnEyesAxis;

        // キー
        bool inputKey;
        bool beforeKeyAction = false;

        // スティック
        Vector2 moveAxis, eyeAxis;
        
        //=================================================
        void Start()
        {
            // マウスカーソル　非表示
            Cursor.visible   = false;
            Cursor.lockState = CursorLockMode.Locked;

            moveAxis = eyeAxis = new Vector2(0, 0);
        }
        void Update()
        {
            // マウスカーソル　表示
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            // 入力の監視
            if (ChangedActionKey()) OnActionButton(inputKey);
            if (IsMoveAxis()) OnMoveAxis(moveAxis);
            if (IsEyesAxis()) OnEyesAxis(eyeAxis);
            // 入力の保存
            UpdateBeforeInput();
        }
        //-------------------------------------------------
        //  監視用
        //-------------------------------------------------
        // アクションキー
        bool ChangedActionKey()
        {
            inputKey = KeyStateAction();
            return inputKey != beforeKeyAction;
        }
        // 移動キー
        bool IsMoveAxis()
        {
            Vector2 beforeAxis = moveAxis;
            moveAxis = KeyStateMove();
            return moveAxis != beforeAxis;
        }
        // 視線キー
        bool IsEyesAxis()
        {
            Vector2 beforeAxis = eyeAxis;
            eyeAxis = KeyStateEyes();
            return eyeAxis != beforeAxis;
        }
        //-------------------------------------------------
        //  キー入力の保存
        //-------------------------------------------------
        void UpdateBeforeInput()
        {
            beforeKeyAction = KeyStateAction();
        }
        //-------------------------------------------------
        //  キー入力の状況
        //-------------------------------------------------
        // アクションキー
        bool KeyStateAction()  { return Input.GetButton("Action"); }
        // 移動キー
        Vector2 KeyStateMove() { return new Vector2(Input.GetAxisRaw("MoveAxis X"), Input.GetAxisRaw("MoveAxis Y")); }
        // 視線キー
        Vector2 KeyStateEyes() { return new Vector2(Input.GetAxisRaw("ViewAxis X"), Input.GetAxisRaw("ViewAxis Y")); }
    }
}