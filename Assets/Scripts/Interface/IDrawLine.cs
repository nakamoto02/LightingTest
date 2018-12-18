using UnityEngine;

namespace Lumiere.Gimick
{
    public interface IDrawLine
    {
        // 線を繋げれる状態か
        bool    IsDraw { get; }

        // 線を繋げる位置
        Vector3 DrawPosition { get; }
    }
}