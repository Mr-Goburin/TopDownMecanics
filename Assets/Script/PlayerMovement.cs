using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    public bool isWalking,onDamage;
    //checa se personagem está andando ou sofrendo dano
    
    public float _speed;
    //determina velocidade do personagem
    
    public int PlayerLife,invisibleTime;
    //Vida do personagem, tempo de invisibilidade após sofrer dano
    
    private PlayerInputs _playerInputs;
    //new input do unity
    
    private Vector2 _currentMovement;
    //movimentação do personagem

    private Rigidbody2D _rigidbody2D;

    public GameObject _hudGameOver;
    //mensagem de game over
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        _playerInputs = new PlayerInputs();//adiciona o input
    }

    private void OnEnable()
    {
        _playerInputs.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputs.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
        if (onDamage)
        {
            StartCoroutine(CallonDamage());
        }

        if (PlayerLife <= 0)//checa se personagem está vivo
        {
            _hudGameOver.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void HandleMovement()//recebe movimentação do jogador
    {
        _currentMovement = _playerInputs.Player.Movement.ReadValue<Vector2>();
        isWalking = (_currentMovement.x != 0 || _currentMovement.y != 0);
        _animator.SetBool("isWalking",isWalking);
        if (_currentMovement.x < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        
        else if (_currentMovement.x > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
    }
    
    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _currentMovement * _speed * Time.deltaTime;//move o personagem com movimentação recebida
    }
    
    IEnumerator CallonDamage()//Ativa o "flash" após sofrer dano
    {

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        for (int i = 0; i < invisibleTime; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        onDamage = false;
    }
}
