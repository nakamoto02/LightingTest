using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lumiere.Gimick;

namespace Lumiere.Player
{
    public class PlayerCore : MonoBehaviour, IDrawLine
    {
        Transform transformCache;

        [SerializeField] Transform lampTransform;

        bool isAction = false;

        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public Vector3 Position { get { return transformCache.position; } }
        public bool IsDraw  { get { return isAction; } }
        public Vector3 DrawPosition { get { return lampTransform.position; } }
        //=================================================
        void Start()
        {
            transformCache = transform;

            IPlayerInput playerInput = GetComponent<IPlayerInput>();
            playerInput.OnActionButton += ChangeActionKeyState;
        }
        //-------------------------------------------------
        //  入力
        //-------------------------------------------------
        void ChangeActionKeyState(bool isPush) { isAction = isPush; }
    }
}