using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 같은 오브젝트의 Graphic(Image 등)이 만들어낸 메쉬를 y좌표 기준으로 전단(shear)시켜
/// 평행사변형 모양으로 보이게 한다.
///
/// 핵심: Image의 Fill(가로 채우기) 로직은 항상 "수직 직선"으로 도형을 자른다.
/// 이 컴포넌트는 Fill이 끝난 뒤의 메쉬(잘린 단면 포함) 전체를 동일한 각도로 기울여주기 때문에,
/// 평행사변형 모양의 바(Background / BlockFill / HPFill)에 전부 똑같은 skewX 값으로 붙이면
/// fillAmount로 잘린 단면도 평행사변형의 다른 변과 같은 각도로 기울어져서 자연스럽게 보인다.
///
/// 사용법:
/// 1. Background, BlockFill, HPFill 오브젝트 각각에 이 스크립트를 추가한다.
/// 2. 세 오브젝트 모두 skewX 값을 동일하게 맞춘다. (다르면 서로 어긋나 보임)
/// 3. fillAmount는 기존 HpBlockBar.cs 스크립트에서 하던 대로 그대로 사용하면 된다.
///    (Fill 계산 로직은 전혀 건드릴 필요 없음)
/// </summary>
[RequireComponent(typeof(Graphic))]
[ExecuteAlways]
public class ParallelogramSkew : UIBehaviour, IMeshModifier
{
    [Tooltip("기울이는 정도 (픽셀 단위). 양수면 위쪽이 오른쪽으로 기울어짐, 음수면 반대 방향.")]
    [SerializeField] private float skewX = 20f;

    private Graphic graphic;

    protected override void OnEnable()
    {
        base.OnEnable();
        graphic = GetComponent<Graphic>();
        graphic.SetVerticesDirty();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (graphic == null) graphic = GetComponent<Graphic>();
        if (graphic != null) graphic.SetVerticesDirty();
    }
#endif

    // 레거시 메서드 (사용하지 않음, VertexHelper 버전만 실제로 동작)
    public void ModifyMesh(Mesh mesh) { }

    public void ModifyMesh(VertexHelper vh)
    {
        if (!isActiveAndEnabled) return;

        Rect rect = ((RectTransform)transform).rect;
        if (rect.height <= 0f) return;

        UIVertex vertex = default;
        int count = vh.currentVertCount;

        for (int i = 0; i < count; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);

            // 0 = 바닥, 1 = 꼭대기 기준으로 현재 정점의 높이 비율을 구함
            float t = Mathf.InverseLerp(rect.yMin, rect.yMax, vertex.position.y);

            Vector3 pos = vertex.position;
            pos.x += Mathf.Lerp(0f, skewX, t);
            vertex.position = pos;

            vh.SetUIVertex(vertex, i);
        }
    }

    /// <summary>런타임에 기울기 값을 바꾸고 싶을 때 사용</summary>
    public void SetSkew(float newSkewX)
    {
        skewX = newSkewX;
        if (graphic == null) graphic = GetComponent<Graphic>();
        graphic.SetVerticesDirty();
    }
}
