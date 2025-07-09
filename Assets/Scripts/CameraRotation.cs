using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    private CinemachineInputAxisController _currentInputAxisController;
    private CinemachineInputAxisController _playerNonCombatInputAxisController;

    private void Start()
    {
        _playerNonCombatInputAxisController = GameObject.FindGameObjectWithTag("PlayerNonCombatCamera").GetComponent<CinemachineInputAxisController>();
        _currentInputAxisController = _playerNonCombatInputAxisController;
    }

    private void Update()
    {
        if (Mouse.current.middleButton.isPressed)
        {
            // Enable the Cinemachine Input Axis Controller to allow camera rotation
            _currentInputAxisController.enabled = true;
        }
        else
        {
            // Disable the Cinemachine Input Axis Controller to stop camera rotation
            _currentInputAxisController.enabled = false;
        }
    }

    public void SwapInputAxisController(CinemachineInputAxisController newCinemachineInputAxisController)
    {
        _currentInputAxisController = newCinemachineInputAxisController;
    }
}