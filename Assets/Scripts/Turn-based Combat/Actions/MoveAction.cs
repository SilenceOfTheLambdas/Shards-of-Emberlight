using System;
using Characters;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Turn_based_Combat.Actions
{
    [CreateAssetMenu(fileName = "MoveAction", menuName = "Turn-based Combat/Actions/Move Action")]
    public class MoveAction : Action
    {
        public int MovementRange;

        public override event Action<Character> OnActionEnd;
        protected override void SetMagicCost(int magicCost) => MagicCost = magicCost;
        protected override void SetStaminaCost(int staminaCost) => StaminaCost = staminaCost;
        protected override void StartAction(Character character)
        {
        }

        public override void UpdateAction(Character character)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame 
                && TbcController.Instance.IsCursorHoveredOverCombatArea())
            {
                MoveCharacterToPositionOnGrid(character);
                // End this action
                EndAction(character);
            }
        }

        protected override void EndAction(Character character)
        {
            character.currentCharacterStaminaPoints -= StaminaCost;
            OnActionEnd?.Invoke(character);
        }
        
        public void MoveCharacterToPositionOnGrid(Character characterToMove)
        {
            var gridCell = TbcController.Instance.activeGridManager.GetGridCellPlayerClickedOn();
            if (gridCell != null)
            {
                if (characterToMove.currentGridCellCharacterIsStandingOn != null)
                    characterToMove.currentGridCellCharacterIsStandingOn.CellIsOccupiedBy = GridCell.OccupiedBy.None;

                var characterNavMeshAgent = characterToMove.GetComponent<NavMeshAgent>();
                characterNavMeshAgent.isStopped = false;
                characterNavMeshAgent.SetDestination(gridCell.gameObject.transform.position);
            
                gridCell.CellIsOccupiedBy =  GridCell.OccupiedBy.PlayerCharacter;
                characterToMove.currentGridCellCharacterIsStandingOn = gridCell;
            }
            else
            {
                Debug.Log("This area is outside of the Combat Area");
            }
        }
    }
}
