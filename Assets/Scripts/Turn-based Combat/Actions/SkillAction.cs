using System;
using Characters;

namespace Turn_based_Combat.Actions
{
    public class SkillAction : Action
    {
        public SkillAction(Turn turn) : base(turn)
        {
            
        }

        public override event Action<Character> OnActionEnd;
        protected override void SetMagicCost(int magicCost) => MagicCost = magicCost;
        protected override void SetStaminaCost(int staminaCost) =>  StaminaCost = staminaCost;
        protected override void StartAction(Character character)
        {
            
        }

        public override void UpdateAction(Character character)
        {
            
        }

        protected override void EndAction(Character character)
        {
            OnActionEnd?.Invoke(character);
        }
    }
}
