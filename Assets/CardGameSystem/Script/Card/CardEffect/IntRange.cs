using UnityEngine;

namespace CardSystem.Effects
{
    /// <summary>
    /// 고정값과 랜덤 범위를 하나의 필드로 표현하기 위한 값 타입.
    /// randomArray 중 값 하나 랜덤 선택
    /// 고정 값 시 값을 하나만 주면 됨.
    /// </summary>
    [System.Serializable]
    public struct IntRange
    {
        public int[] randomArray;
        
        public readonly int GetValue()
        {
            return randomArray.Length == 1 ? randomArray[0] : randomArray[Random.Range(0, randomArray.Length)];
        }
    }
}
