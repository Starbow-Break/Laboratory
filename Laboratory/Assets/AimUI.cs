using System;
using System.Collections;
using UnityEngine;

public class AimUI : MonoBehaviour
{
    [SerializeField, Min(1)] float biggerSizeMultiplier;
    [SerializeField, Min(0.01f)] float sizeSpeed;

    Coroutine sizeChangeCoroutine = null;
    RectTransform aimRect;
    Vector2 aimOriginSize;
    float aimScale;

    public void Awake()
    {
        aimRect = GetComponent<RectTransform>();
        aimOriginSize = aimRect.sizeDelta;
        aimScale = 1.0f;
    }

    public void SetBigger(bool value)
    {
        if(sizeChangeCoroutine != null)
        {
            sizeChangeCoroutine = null;
        }

        if (value) {
            sizeChangeCoroutine = StartCoroutine(ChangeSize(biggerSizeMultiplier, sizeSpeed));
        }
        else {
            sizeChangeCoroutine = StartCoroutine(ChangeSize(1.0f, -sizeSpeed));
        }
    }

    private IEnumerator ChangeSize(float scaleTarget, float speed)
    {
        // while문의 지속 조건을 결정하는 delegate 설정
        Predicate<float> predicate = speed > 0 
            ? (currentScale) => currentScale < scaleTarget 
            : (currentScale) => currentScale > scaleTarget;

        // 사이즈 변화
        while(predicate(aimScale))
        {
            yield return null;
            aimScale += Time.deltaTime * speed;
            aimRect.sizeDelta = aimOriginSize * aimScale;
        }

        // sizeChangeCoroutine 비워주기
        sizeChangeCoroutine = null;
    }
}
