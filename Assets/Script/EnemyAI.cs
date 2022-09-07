using System;
using System.Collections;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed,delayShoot;
    //velocidade do inimigo e o tempo de tiro para proximo tiro
    private GameObject _player;
    private Animator _animator;
    [SerializeField]private Transform _enemyaim;
    //ponto de spawn do tiro

    public bool onDamage,onChase,onAttack;
        // checa quais ação estão ativa
    public int enemyLife;
    [SerializeField]private float distantPlayer,enemyVision,enemyLimitChase,rangeShoot;
    //checa distancia do jogador e limite de persiguição do inimigo

    private bool shootReady = true;
    //evita disparos continuos

    [SerializeField]private GameObject _shoot;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distantPlayer = Vector3.Distance(gameObject.transform.position, _player.transform.position);
        Vector3 dir = transform.position - _player.transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        _enemyaim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (onChase)
        {
            if (distantPlayer > rangeShoot)
            {
                _animator.SetBool("isWalking",true);
                
                onAttack = false;
            }
            else
            {
                _animator.SetBool("isWalking",false);
                onAttack = true;
            }
        }

        else
        {
            _animator.SetBool("isWalking",false);
            if (distantPlayer < enemyVision)
            {
                onChase = true;
            }
        }
        
        if (distantPlayer > enemyLimitChase)
        {
            onChase = false;
        }
        if (_player.transform.position.x < transform.position.x)//aponta o inimigo para o lado do jogador
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        else
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (onDamage)
        {
            onDamage = false;
            StartCoroutine(CallonDamage());
        }

        if (enemyLife <= 0)
        {
            Destroy(gameObject);
        }

        if (onAttack)
        {
            if (_player.activeSelf)
            {
                if (shootReady)
                {
                    StartCoroutine(CallShooting());
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (onChase && !onAttack)//move o inimigo direção jogador
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _player.transform.position, speed * Time.deltaTime);
        }
    }

    IEnumerator CallonDamage()//Ativa o "flash" após sofrer dano
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 5; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    IEnumerator CallShooting()//Chama o tiro e aguarda proximo
    {
        shootReady = false;
        Shooting();
        yield return new WaitForSeconds(delayShoot);
        shootReady = true;
    }

    private void Shooting()//cria o tiro
    {
        GameObject shoot = Instantiate(_shoot, _enemyaim.position, _enemyaim.rotation);
        shoot.GetComponent<Bullet>().origin = 1;
    }
}
