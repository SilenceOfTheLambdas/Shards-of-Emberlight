using Turn_based_Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Characters
{
    public class FollowerMovementController : MonoBehaviour
    {
        private static readonly int Velocity = Animator.StringToHash("velocity");
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private Transform _playerTransform;
        [FormerlySerializedAs("_followOffset")] [SerializeField] private float followOffset = 2f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void FixedUpdate()
        {
            // Only follow the main character when outside of combat
            if (_playerTransform != null && !TbcController.Instance.isPlayerInCombat)
            {
                Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;
                Vector3 targetPosition = _playerTransform.position - directionToPlayer * followOffset;
                _navMeshAgent.SetDestination(targetPosition);
            }
            _animator.SetFloat(Velocity, _navMeshAgent.velocity.magnitude);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _navMeshAgent.isStopped = true;
            }
        }
    
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _navMeshAgent.isStopped = false;
            }
        }
    }
}
