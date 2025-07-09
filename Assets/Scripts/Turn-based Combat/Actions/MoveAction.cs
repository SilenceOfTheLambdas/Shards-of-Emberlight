using System;
using Characters;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Turn_based_Combat.Actions
{
    public class MoveAction : Action
    {
        public int MovementRange;

        public MoveAction(Turn turn) : base(turn) {}

        public override event Action<Character> OnActionEnd;
        protected override void SetMagicCost(int magicCost) => MagicCost = magicCost;
        protected override void SetStaminaCost(int staminaCost) => StaminaCost = staminaCost;
        protected override void StartAction(Character character)
        {
            Debug.Log("Started Move Action");
            SetStaminaCost(10);
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
