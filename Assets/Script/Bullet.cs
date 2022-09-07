using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    private GameObject _player;
    public bool mirror;
    [SerializeField] private int damage;
    public int origin;
    //0 = player, 1 = enemy

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        if (origin == 0)
        {
            print("player");
            if (_player.transform.localScale.x >= 0)
            {
                mirror = false;
            }
        
            else if (_player.transform.localScale.x <= 0)
            {
                mirror = true;
            }
        }

        else if (origin == 1)
        {
            mirror = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!mirror)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
        else
        {
            transform.Translate(-Vector3.right * Time.deltaTime * speed);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (origin == 0)
        {
            if (other.tag != "Player")
            {
                if (other.tag == "Enemy")
                {
                    EnemyAI enemyColider = other.GetComponent<EnemyAI>();
                    enemyColider.enemyLife =enemyColider.enemyLife -damage;
                    enemyColider.onDamage = true;
                }
                Destroy(gameObject);
            }
        }
        
        else if (origin == 1)
        {
            if (other.tag != "Enemy")
            {
                if (other.tag == "Player")
                {
                    PlayerMovement playerCollider = other.GetComponent<PlayerMovement>();
                    if (!playerCollider.onDamage)
                    {
                        playerCollider.onDamage = true;
                        playerCollider.PlayerLife =playerCollider.PlayerLife - damage;
                        Destroy(gameObject);
                    }
                }

                else
                {
                    Destroy(gameObject);
                }
                
            }
        }
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    
}
