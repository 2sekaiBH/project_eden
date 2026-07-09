using UnityEngine;

namespace CardSystem.Runtime
{
    /// <summary>
    /// 카드/이펙트 실행 시 필요한 모든 참조를 담아 전달하는 컨텍스트.
    /// 새로운 정보가 필요해지면 여기에만 필드를 추가하면 되므로,
    /// 이펙트 클래스들의 시그니처를 계속 바꾸지 않아도 된다.
    /// </summary>
    public class CardContext
    {
        
        public ICardActor caster { get; }
        public ICardActor target { get; }

        
        // 예: 현재 라운드 수, 랜덤 시드, 전투 로그 등 확장 여지
        public int CurrentRound { get; set; }

        public CardContext(ICardActor caster, ICardActor target)
        {
            this.caster = caster;
            this.target = target;
        }
    }
}
