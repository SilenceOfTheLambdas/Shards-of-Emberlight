using Characters;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Turn_based_Combat
{
    public class TbcController : MonoBehaviour
    {
        private static TbcController _instance;
        public static TbcController Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindFirstObjectByType<TbcController>();

                if (_instance == null)
                {
                    Debug.LogError("There needs to be one active TbcController script on a GameObject in your scene.");
                }
                return _instance;
            }
        }

        public enum DamageType
        {
            Physical,
            Magic
        }
        
        /// <summary>
        /// The list of Turns for this combat session
        /// </summary>
        public TurnQueue Turns = new();
        
        public Camera playerCamera;
        public Character activeCharacter;
        public PlayerCombatController playerCombatController;
        public bool isPlayerInCombat = false;
        public GameObject activePlayerIndicatorPrefab;
    
        [SerializeField] private CinemachineCamera playerNonCombatCamera;
        [SerializeField] public Button EndTurnButton;
    
        private GameObject _combatCamera;
        [FormerlySerializedAs("_activeGridManager")] public GridManager activeGridManager;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    
        private void Start()
        {
            // The main player has entered a combat area
            Assert.IsNotNull(playerCombatController);
            playerCombatController.OnEnterCombatState += EnterCombatState;
            playerCombatController.OnExitCombatState += ExitCombatState;
        }

        private void EnterCombatState(GameObject playerGameObject, GameObject enemyGameObject)
        {
            playerGameObject.GetComponent<NavMeshAgent>().SetDestination(playerGameObject.transform.position);
            // TODO: Move player and Enemy(ies) within combat range

            //Switch to the combat camera
            playerNonCombatCamera.gameObject.SetActive(false);
            _combatCamera = enemyGameObject.transform.Find("CombatAreaCamera").gameObject;
            _combatCamera.SetActive(true);
        
            playerCombatController.gameObject.GetComponent<CameraRotation>().SwapInputAxisController(_combatCamera?.GetComponent<CinemachineInputAxisController>());
            activeGridManager = enemyGameObject.GetComponentInChildren<GridManager>();
            
            // Add the player to the Turns list
            Turns.Enqueue(playerGameObject.GetComponent<Character>(), new PlayerTurn());

            // 1st: Go through each enemy character involved in this combat
            foreach (var enemyCharacter in activeGridManager.enemyCharacters)
            {
                // 2nd: Add each character to the Turns list
                Turns.Enqueue(enemyCharacter, new EnemyTurn());
            }
            
            // 3rd: Then add any followers
            var followers = GameObject.FindGameObjectsWithTag("Follower");
            foreach (var follower in followers)
            {
                var followerTurn = new PlayerTurn();
                Turns.Enqueue(follower.GetComponent<Character>(), followerTurn);
            }
            
            
            // Begin First Turn
            if (Turns.Count > 0)
            {
                Turns.Sort();
                BeginFirstTurn();
            }
        }

        private void Update()
        {
            if (isPlayerInCombat && Turns.Count > 0)
            {
                var (character, theTurn) = Turns.Dequeue();
                theTurn.UpdateTurn(character, theTurn);
                theTurn.OnEndTurn += SwitchToNextCharacter;
            }
        }

        private void BeginFirstTurn()
        {
            // Get the first character in Turns
            var firstTurn = Turns.Peek();
            firstTurn.turn.BeginTurn(firstTurn.character, firstTurn.turn);

            if (firstTurn.character.gameObject.CompareTag("Player"))
                firstTurn.character.OnPlayerDeath += PlayerDeath;
            if (firstTurn.character.gameObject.CompareTag("Follower"))
                firstTurn.character.OnFollowerDeath += FollowerDeath;
            if (firstTurn.character.gameObject.CompareTag("Enemy"))
                firstTurn.character.OnEnemyDeath += EnemyDeath;
        }


        private void SwitchToNextCharacter(Character character, Turn turn)
        {
            Turns.Requeue(character, turn);
            EndTurnButton.onClick.RemoveAllListeners();
            BeginFirstTurn();
        }
        
        private void PlayerDeath(Character playerCharacter)
        {
            Debug.Log("Player Death");
            // TODO: Reload scene
            Turns.Clear();
            playerCharacter.OnPlayerDeath -= PlayerDeath;
        }

        private void FollowerDeath(Character followerCharacter)
        {
            Debug.Log("Follower Death");
            Turns.Remove(followerCharacter);
        }

        private void EnemyDeath(Character enemyCharacter)
        {
            Debug.Log("Enemy Death");
            Turns.Remove(enemyCharacter);
        }

        private void ExitCombatState(GameObject playerGameObject, GameObject combatArea)
        {
            Destroy(combatArea);
            isPlayerInCombat = false;
            Turns.Clear();
        }
        
        public bool IsCursorHoveredOverCombatArea()
        {
            var screenPosition = Mouse.current.position.ReadValue();
            var ray = playerCamera.ScreenPointToRay(screenPosition);
            Physics.Raycast(ray, out RaycastHit hit, 100f, activeGridManager.gridCellMask);
        
            return hit.collider != null && hit.collider.gameObject.CompareTag("CombatArea");
        }

    }
}
