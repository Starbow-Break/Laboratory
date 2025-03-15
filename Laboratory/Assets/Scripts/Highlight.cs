using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;      // 오브젝트에서 사용하는 렌더러들
    [SerializeField] Color highlightColor = Color.white;    // 하이라이트 색

    private List<Material> materials;   // 사용하는 모든 머터리얼들

    private void Awake()
    {
        // 렌더러에 할당된 모든 머터리얼들을 추출
        materials = new List<Material>();
        foreach(var renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void SetHighlight(bool value)
    {
        if (value)
        {
            // emission 키워드 활성화 및 emission color를 highlight color로 변경
            foreach (var material in materials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", highlightColor);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                // emission 키워드 비활성화
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
