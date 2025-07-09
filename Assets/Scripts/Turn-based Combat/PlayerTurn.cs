using System;
using System.Linq;
using Characters;
using Turn_based_Combat.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Turn_based_Combat
{
    public class PlayerTurn : Turn
    {
        public override event Action<Character> OnInitiateAction;
        public override event Action<Character, Turn> OnEndTurn;
        private Turn _playersCurrentTurn;
        
        public override void BeginTurn(Character character, Turn turn)
        {
            _playersCurrentTurn = turn;
            GameObject.Instantiate(TbcController.Instance.activePlayerIndicatorPrefab, 
                character.transform.position + (Vector3.up * 4), Quaternion.Euler(90, 0, 90), character.gameObject.transform);

        }

        public override void UpdateTurn(Character character, Turn turn)
        {
            if (character.currentCharacterStaminaPoints > 0)
            {
                // Keep allowing the player to queue actions
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    Debug.Log("Move Key Pressed");
                    // Add the action to the Actions list
                    var moveAction = new MoveAction(_playersCurrentTurn);
                    Actions.Enqueue(moveAction);
                    OnInitiateAction?.Invoke(character);
                    moveAction.OnActionEnd += NextAction;
                }
            }
            else
            {
                // // Character is out of AP, so end this character's Turn
                // TbcController.Instance.isPlayerInCombat = false;
                // NextAction(character);
            }

            if (Actions.Count > 0)
            {
                var firstAction = Actions.First();
                firstAction.UpdateAction(character);
            }

            // If this Character has become not living 💀
            if (character.currentCharacterHealth <= 0)
            {
                Actions.Clear();
                // Send event signal to end this character's turn
                OnEndTurn?.Invoke(character, _playersCurrentTurn);
            }
        }

        protected override void NextAction(Character character)
        {
            Debug.Log("Next Action");
            Actions.Dequeue();

            if (Actions.Count <= 0)
            {
                // Send event signal to end this character's turn
                OnEndTurn?.Invoke(character, _playersCurrentTurn);
            }
        }
    }
}
