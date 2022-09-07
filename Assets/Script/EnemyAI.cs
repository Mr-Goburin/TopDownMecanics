using System.Collections;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float speed,delayShoot;
    private GameObject _player;
    private Animator _animator;
    public Transform _enemyaim;

    public bool onDamage,onChase,onAttack;
    public int enemyLife;
    public float distantPlayer,enemyVision,enemyLimitChase,rangeShoot;

    private bool shootReady = true;

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
        _animator.SetBool("isWalking",true);
        if (onChase)
        {
            if (distantPlayer > rangeShoot)
            {
                _animator.SetBool("isWalking",true);
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, speed * Time.deltaTime); 
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
        if (_player.transform.position.x < transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
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
    
    IEnumerator CallonDamage()
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
    
    IEnumerator CallShooting()
    {
        shootReady = false;
        Shooting();
        yield return new WaitForSeconds(delayShoot);
        shootReady = true;
    }

    private void Shooting()
    {
        GameObject shoot = Instantiate(_shoot, _enemyaim.position, _enemyaim.rotation);
        shoot.GetComponent<Bullet>().origin = 1;
    }
}
