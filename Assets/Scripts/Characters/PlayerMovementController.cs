using Turn_based_Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerMovementController : MonoBehaviour
    {
        private static readonly int PlayerVelocity = Animator.StringToHash("Player_Velocity");
    
        private NavMeshAgent _playerNavMeshAgent;
        private Animator _playerAnimator;
        private PlayerCombatController _playerCombatController;
    
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask groundMask;
        
        [Header("Movement Attributes")]
        [SerializeField] private float playerMovementSpeed;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _playerNavMeshAgent = GetComponent<NavMeshAgent>();
            _playerAnimator = GetComponent<Animator>();
            _playerCombatController = GetComponent<PlayerCombatController>();
        }

        void FixedUpdate()
        {
            if (!TbcController.Instance.isPlayerInCombat)
            {
                HandlePlayerNonCombatMovement();
            }
            _playerAnimator.SetFloat(PlayerVelocity, _playerNavMeshAgent.velocity.magnitude);
        }

        /// <summary>
        /// Handles the player character's movement *outside* of combat.
        /// </summary>
        private void HandlePlayerNonCombatMovement()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                _playerNavMeshAgent.destination = GetMouseCursorWorldPosition();
            }
        }

        private Vector3 GetMouseCursorWorldPosition()
        {
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(screenPosition);

            return Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask) ? hit.point : Vector3.zero;
        }
    }
}
