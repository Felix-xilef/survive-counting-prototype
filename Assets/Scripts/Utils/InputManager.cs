using UnityEngine;

public static class InputManager {

    public static float Horizontal => Input.GetAxis(InputAxis.Horizontal);

    public static float Vertical => Input.GetAxis(InputAxis.Vertical);

    public static bool Jump => Input.GetButtonDown(InputAxis.Jump);
}
