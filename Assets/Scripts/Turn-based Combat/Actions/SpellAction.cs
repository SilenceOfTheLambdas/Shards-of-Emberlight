using System;
using Characters;
using UnityEngine;

namespace Turn_based_Combat.Actions
{
    [CreateAssetMenu(fileName = "SpellAction", menuName = "Turn-based Combat/Actions/Spell Action")]
    public class SpellAction : Action
    {
        public override event Action<Character> OnActionEnd;
        protected override void SetMagicCost(int magicCost) => MagicCost = magicCost;
        protected override void SetStaminaCost(int staminaCost) => StaminaCost = staminaCost;
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
