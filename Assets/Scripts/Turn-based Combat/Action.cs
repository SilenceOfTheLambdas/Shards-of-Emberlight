using System;
using Characters;
using UnityEngine;

namespace Turn_based_Combat
{
    [CreateAssetMenu(fileName = "Action", menuName = "Turn-based Combat/Actions/")]
    public abstract class Action : ScriptableObject
    {
        public enum ActionType
        {
            None,
            Move,
            Skill,
            Spell
        }
        public ActionType actionType;
        public string actionName;
        
        public abstract event Action<Character> OnActionEnd;
        
        public int MagicCost;
        public int StaminaCost;

        protected Turn Turn;

        public void Initialize(Turn turn)
        {
            this.Turn = turn;
            turn.OnInitiateAction += StartAction;
        }

        protected virtual void OnDisable()
        {
            if (Turn != null)
                Turn.OnInitiateAction -= StartAction;
        }
        protected abstract void SetMagicCost(int magicCost);
        protected abstract void SetStaminaCost(int staminaCost);
        
        protected abstract void StartAction(Character character);
        
        public abstract void UpdateAction(Character character);
        
        protected abstract void EndAction(Character character);
    }
}
