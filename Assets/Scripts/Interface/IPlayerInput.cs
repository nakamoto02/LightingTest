using UnityEngine;

namespace Lumiere.Player
{

    public delegate void ButtonEventHandler(bool isPush);
    public delegate void AxisEventHandler(Vector2 axis);

    public interface IPlayerInput
    {
        event ButtonEventHandler OnActionButton;
        event AxisEventHandler   OnMoveAxis;
        event AxisEventHandler   OnEyesAxis;
    }
}