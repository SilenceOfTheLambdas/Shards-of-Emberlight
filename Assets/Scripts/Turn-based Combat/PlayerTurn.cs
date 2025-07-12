using System;
using System.Collections.Generic;
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
        private int _numberOfActionsAvailable;
        
        /// <summary>
        /// Stores a temporary list of Actions available to this player character.
        /// Action -> The action,
        /// bool -> Has this Action been used this turn?
        /// </summary>
        private Dictionary<Action, bool> _currentCharacterActions;
        
        public override void BeginTurn(Character character, Turn turn)
        {
            Actions?.Clear();
            _playersCurrentTurn = turn;
            
            GameObject.Instantiate(TbcController.Instance.activePlayerIndicatorPrefab, 
                character.transform.position + (Vector3.up * 4), Quaternion.Euler(90, 0, 90), character.gameObject.transform);
            _currentCharacterActions = new Dictionary<Action, bool>();

            if (character.actionsAvailableToCharacter.Count > 0)
            {
                foreach (var action in character.actionsAvailableToCharacter)
                {
                    _currentCharacterActions.Add(action, false);
                    _numberOfActionsAvailable++;
                }
            }
            
            if (TbcController.Instance.EndTurnButton != null)
            {
                TbcController.Instance.EndTurnButton.onClick.AddListener(() =>
                {
                    Debug.Log("Button Clicked");
                    Actions.Clear();
                    OnEndTurn?.Invoke(character, _playersCurrentTurn);
                });
            }
        }

        public override void UpdateTurn(Character character, Turn turn)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                if (HandlePlayerMovementActions(character, turn))
                    // Initiate The Action
                    OnInitiateAction?.Invoke(character);

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

        /// <summary>
        /// Handles the player movement Actions. It checks for appropriate inputs and then activates the corresponding
        /// Action.
        /// </summary>
        /// <param name="character">The Player Character</param>
        /// <param name="turn">This turn</param>
        /// <returns>True if a MoveAction was fulfilled, false otherwise.</returns>
        private bool HandlePlayerMovementActions(Character character, Turn turn)
        {
            // Keep allowing the player to queue actions
            if (character.currentCharacterStaminaPoints > 0)
            {
                foreach (var (action, hasBeenUsed) in _currentCharacterActions)
                {
                    // Check for Free Move Skill
                    MoveAction moveAction;
                    if (action.actionType == Action.ActionType.Move &&
                        action.actionName == "Free Move Skill" && hasBeenUsed == false)
                    {
                        moveAction = (MoveAction) action;
                        ScriptableObject.CreateInstance<MoveAction>();
                        moveAction.Initialize(turn);
                        Actions.Enqueue(moveAction);
                        moveAction.OnActionEnd += NextAction;
                        _currentCharacterActions[moveAction] = true;
                        _numberOfActionsAvailable--;
                        return true;
                    }
                    
                    // Check for paid Move Skill
                    if (action.actionType == Action.ActionType.Move &&
                        action.actionName == "Move Skill" && hasBeenUsed == false)
                    {
                        moveAction = (MoveAction) action;
                        ScriptableObject.CreateInstance<MoveAction>();
                        moveAction.Initialize(turn);
                        Actions.Enqueue(moveAction);
                        moveAction.OnActionEnd += NextAction;
                        _currentCharacterActions[moveAction] = true;
                        _numberOfActionsAvailable--;
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void NextAction(Character character)
        {
            Debug.Log("Next Action");
            if (Actions.Count > 0)
            {
                Debug.Log("Getting rid of action: " + Actions.Peek().actionName);
                Actions.Dequeue();
            }
            
            // if (_numberOfActionsAvailable <= 0)
            // {
            //     // Send event signal to end this character's turn
            //     OnEndTurn?.Invoke(character, _playersCurrentTurn);
            // }
        }
    }
}
