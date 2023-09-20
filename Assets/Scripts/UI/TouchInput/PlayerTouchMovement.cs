using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField] private Vector2 JoystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick Joystick;
    [SerializeField] private Player Player;

    private Finger MovementFinger;
    private Vector2 movementAmount;

    public Vector2 MovementAmount
    {
        get
        {
            return movementAmount;
        }
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 3f;

            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(
                    currentTouch.screenPosition,
                    Joystick.RectTransform.anchoredPosition
                ) > maxMovement)
            {
                knobPosition = (
                    currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition
                    ).normalized
                    * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            //movementAmount = knobPosition / maxMovement;
            movementAmount = knobPosition / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            //movementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        //length of the screen
        if (MovementFinger == null && TouchedFinger.screenPosition.x <= Screen.width)
        {
            MovementFinger = TouchedFinger;
            //movementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < JoystickSize.x / 2)
        {
            StartPosition.x = JoystickSize.x / 2;
        }
        else if (StartPosition.x > Screen.width - JoystickSize.x / 2)
        {
            StartPosition.y = Screen.width - JoystickSize.x / 2;
        }

        if (StartPosition.y < JoystickSize.y / 2)
        {
            StartPosition.y = JoystickSize.y / 2;
        }
        //else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
        //{
        //    StartPosition.y = Screen.height - JoystickSize.y / 2;
        //}

        return StartPosition;
    }

    private void FixedUpdate()
    {
        //Vector3 scaledMovement = Player.speed * Time.deltaTime * new Vector3(MovementAmount.x, 0, MovementAmount.y);
        Vector3 scaledMovement = 100f * Time.fixedDeltaTime * new Vector3(movementAmount.x, movementAmount.y, 0);

        float angle = Rotate(scaledMovement);

        if (!float.IsPositiveInfinity(angle))
        {
            Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, Quaternion.Euler(new Vector3(0, 0, -angle)), Time.deltaTime * 100f);
        }

        //Player.Movement(scaledMovement.x, scaledMovement.y);
        //snakeM.SnakeMovement(scaledMovement.x, scaledMovement.y);
    }

    private float Rotate(Vector3 scaledMovement)
    {
        if (scaledMovement == Vector3.zero) return Mathf.Infinity;

        return Mathf.Atan2(scaledMovement.x, scaledMovement.y) * Mathf.Rad2Deg;
    }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        if (MovementFinger != null)
        {
            GUI.Label(new Rect(10, 35, 500, 20), $"Finger Start Position: {MovementFinger.currentTouch.startScreenPosition}", labelStyle);
            GUI.Label(new Rect(10, 65, 500, 20), $"Finger Current Position: {MovementFinger.currentTouch.screenPosition}", labelStyle);
        }
        else
        {
            GUI.Label(new Rect(10, 35, 500, 20), "No Current Movement Touch", labelStyle);
        }

        GUI.Label(new Rect(10, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    }
}
