using System;
using Characters;

namespace Turn_based_Combat
{
    public abstract class Action
    {
        public enum ActionType
        {
            None,
            Move,
            Skill,
            Spell
        }
        public ActionType actionType;
        
        public abstract event Action<Character> OnActionEnd;
        
        public int MagicCost;
        public int StaminaCost;

        protected Turn turn;

        protected Action(Turn turn)
        {
            turn.OnInitiateAction += StartAction;
        }
        protected abstract void SetMagicCost(int magicCost);
        protected abstract void SetStaminaCost(int staminaCost);
        
        protected abstract void StartAction(Character character);
        
        public abstract void UpdateAction(Character character);
        
        protected abstract void EndAction(Character character);
    }
}
