using UnityEngine;

namespace LatterProject
{
    public struct InputState
    {
        public Vector2 axis_L;
        public Vector2 axis_R;

        public bool action;
    }

    interface ICharaInput
    {
        InputState GetInputState();
    }
}