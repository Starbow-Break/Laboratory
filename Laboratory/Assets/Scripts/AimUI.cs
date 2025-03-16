using System;
using System.Collections;
using UnityEngine;

public class AimUI : MonoBehaviour
{
    [SerializeField, Min(1)] float biggerSizeMultiplier;

    RectTransform aimRect;
    Vector2 aimOriginSize;

    public void Awake()
    {
        aimRect = GetComponent<RectTransform>();
        aimOriginSize = aimRect.sizeDelta;
    }

    public void SetBigger(bool value)
    {
        if (value) 
        {
            aimRect.sizeDelta = aimOriginSize * biggerSizeMultiplier;
        }
        else 
        {
            aimRect.sizeDelta = aimOriginSize;
        }
    }
}
