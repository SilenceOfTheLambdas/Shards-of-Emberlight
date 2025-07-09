using System;
using System.Collections.Generic;
using Characters;

namespace Turn_based_Combat
{
    /// <summary>
    /// Handles a 'Turn' within a combat scenario. Stores a list of Actions for a given character.
    /// <remarks><see cref="Actions"/></remarks>
    /// </summary>
    public abstract class Turn
    {
        public Queue<Action> Actions;
        public abstract event Action<Character> OnInitiateAction;
        public abstract event Action<Character, Turn> OnEndTurn;
        
        public Turn()
        {
            Actions = new Queue<Action>();
        }

        public abstract void BeginTurn(Character character, Turn turn);

        public abstract void UpdateTurn(Character character, Turn turn);
        
        protected abstract void NextAction(Character character);
    }
}
