using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private PlayerInputs _playerInputs;
    [SerializeField]private GameObject _shoot;
    
    private void Awake()
    {
        _playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        _playerInputs.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputs.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _playerInputs.Player.Shoot.started += ShootOnstarted;
    }

    private void ShootOnstarted(InputAction.CallbackContext obj)
    {
        GameObject shoot = Instantiate(_shoot, transform.position, transform.rotation);
        shoot.GetComponent<Bullet>().origin = 0;
    }
}
