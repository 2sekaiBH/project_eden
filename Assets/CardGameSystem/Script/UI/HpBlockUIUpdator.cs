using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBlockUIUpdator : MonoBehaviour
{

    [Header("UI 참조")]
    [SerializeField] private Image hpFillImage;
    [SerializeField] private Image blockFillImage;
    [SerializeField] private TextMeshProUGUI blockOverflowText;

    [Header("연출")]
    [SerializeField] private float fillAnimSpeed = 8f; // 초당 fillAmount 변화 속도 (0~1 기준)

    private int currentHp;
    private int currentBlock;

    private Coroutine refreshAnimCo;

    private int maxHp; 

    /// <summary>
    /// 전투 시작 등 최초 세팅. 애니메이션 없이 즉시 반영.
    /// </summary>
    public void Initialize(int hpMax)
    {
        maxHp = hpMax;
        currentHp = hpMax;
        currentBlock = 0;

        RefreshImmediate();
    }

    private void CalcFractions(out float hpFrac, out float blockFrac)
    {
        hpFrac = maxHp <= 0 ? 0f : (float)currentHp / maxHp;
        hpFrac = Mathf.Clamp01(hpFrac);

        float rawBlockFrac = maxHp <= 0 ? 0f : (float)currentBlock / maxHp;
        blockFrac = Mathf.Clamp(rawBlockFrac, 0f, 1f - hpFrac); // 바를 넘치는 만큼은 시각적으로 캡
    }


    private void RefreshImmediate()
    {
        CalcFractions(out float hpFrac, out float blockFrac);

        hpFillImage.fillAmount = hpFrac;
        blockFillImage.fillAmount = hpFrac + blockFrac;

        UpdateOverflowText(hpFrac, blockFrac);
    }


    public void RefreshAnimated(int currentHp, int currentBlock)
    {
        this.currentHp = currentHp;
        this.currentBlock = currentBlock;

        CalcFractions(out float hpFrac, out float blockFrac);

        Debug.Log($"refreshAnimCo : {refreshAnimCo}");
        if (refreshAnimCo != null) StopCoroutine(refreshAnimCo);
        refreshAnimCo = StartCoroutine(AnimateSequential(hpFrac, blockFrac));

        UpdateOverflowText(hpFrac, blockFrac);
    }

    /// <summary>
    /// block 애니메이션이 완전히 끝난 뒤에 hp 애니메이션을 시작한다.
    /// (block 소모 -> 다 소모된 뒤 hp 감소 하는 연출을 명확히 보여주기 위함)
    /// </summary>
    private IEnumerator AnimateSequential(float hpTarget, float blockFrac)
    {
        float blockTarget = hpTarget + blockFrac;

        yield return AnimateFill(blockFillImage, blockTarget);
        yield return AnimateFill(hpFillImage, hpTarget);
    }

    private IEnumerator AnimateFill(Image img, float target)
    {
        while (!Mathf.Approximately(img.fillAmount, target))
        {
            img.fillAmount = Mathf.MoveTowards(img.fillAmount, target, fillAnimSpeed * Time.deltaTime);
            yield return null;
        }
        img.fillAmount = target;
    }

    /// <summary>
    /// block이 (maxHp - 현재hp)를 넘어서 바에 다 표시하지 못할 때, 넘치는 수치를 텍스트로 보여준다.
    /// blockOverflowText를 연결하지 않았다면 아무 동작도 하지 않는다.
    /// </summary>
    private void UpdateOverflowText(float hpFrac, float blockFrac)
    {
        if (blockOverflowText == null) return;

        float actualBlockFrac = maxHp <= 0 ? 0f : (float)currentBlock / maxHp;
        float overflow = actualBlockFrac - blockFrac;

        if (overflow > 0.001f)
        {
            int overflowValue = Mathf.RoundToInt(overflow * maxHp);
            blockOverflowText.text = $"+{overflowValue}";
            blockOverflowText.gameObject.SetActive(true);
        }
        else
        {
            blockOverflowText.gameObject.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
