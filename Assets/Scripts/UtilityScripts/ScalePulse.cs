using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePulse : MonoBehaviour
{

    private float timeElapsed;
    [SerializeField] private float lerpDuration = 3;
    private Vector3 startValue = Vector3.one;
    [SerializeField] private Vector3 endValue;
    //float valueToLerp;
    //AnimationCurve _curve;

    // Start is called before the first frame update
    private void OnEnable()
    {
        Lerp();
    }

    void Lerp()
    {
        if (timeElapsed < lerpDuration)
        {
            transform.localScale = Vector3.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            transform.localScale = endValue;
        }
    }
}
