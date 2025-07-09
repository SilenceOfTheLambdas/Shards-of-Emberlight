using System;
using Characters;

namespace Turn_based_Combat
{
    public class EnemyTurn : Turn
    {
        public override event Action<Character> OnInitiateAction;
        public override event Action<Character, Turn> OnEndTurn;

        public override void BeginTurn(Character character, Turn turn)
        {
            
        }

        public override void UpdateTurn(Character character, Turn turn)
        {
            
        }

        protected override void NextAction(Character character)
        {
            
        }
    }
}
