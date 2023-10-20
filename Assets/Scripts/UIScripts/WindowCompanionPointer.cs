using UnityEngine;

public class WindowCompanionPointer : MonoBehaviour
{
    [SerializeField] private GameObject _pointer;
    [SerializeField] private float _borderSize;

    private GameObject _targetObject;
    private RectTransform _pointerRectTransform;

    public GameObject TargetObject
    {
        get => _targetObject;
        set => _targetObject = value;
    }

    private void Start()
    {
        _pointerRectTransform = _pointer.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 toPosition = _targetObject.transform.position;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0;

        Vector3 dir = toPosition - fromPosition;
        float angle = GetAngleFromVectorFloat(dir);
        _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(_targetObject.transform.position);

        bool isOffScreen = targetPositionScreenPoint.x <= _borderSize || targetPositionScreenPoint.x
            >= Screen.width - _borderSize || targetPositionScreenPoint.y <= _borderSize || targetPositionScreenPoint.y >= Screen.height - (_borderSize * 2);

        if (isOffScreen)
        {
            _pointer.SetActive(true);
            ClampPointer(targetPositionScreenPoint);
        }
        else
        {
            _pointer.SetActive(false);
        }
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg % 360;
        return n;
    }

    private void ClampPointer(Vector3 targetScreenPoint)
    {
        Vector3 cappedTargetScreenPosition = targetScreenPoint;
        if (cappedTargetScreenPosition.x <= _borderSize)
        {
            cappedTargetScreenPosition.x = _borderSize;
        }
        if (cappedTargetScreenPosition.x >= Screen.width - _borderSize)
        {
            cappedTargetScreenPosition.x = Screen.width - _borderSize;
        }
        if (cappedTargetScreenPosition.y <= _borderSize)
        {
            cappedTargetScreenPosition.y = _borderSize;
        }
        if (cappedTargetScreenPosition.y >= Screen.height - (_borderSize * 2))
        {
            cappedTargetScreenPosition.y = Screen.height - (_borderSize * 2);
        }

        _pointerRectTransform.position = cappedTargetScreenPosition;
        _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x,
            _pointerRectTransform.localPosition.y, 0);
    }
}
