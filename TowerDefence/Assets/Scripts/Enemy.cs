using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private GameObject[] wayPoints;
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int rewardAmmount;



    private float navagationUpdate;
    private Transform enemy;
    private int target = 0;   
    private bool isDead = false;
    private Animator anim;
    
    //private Collider2D enemyCollider;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        //enemyCollider.GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        GameManager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (wayPoints != null && !isDead)  
        {


            if (target < wayPoints.Length)
            {
                //enemy.position = wayPoints[target].transform.position;
                enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].transform.position,Time.deltaTime); //navigationTime);
            }
            else 
            {
                enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, Time.deltaTime);
            }
            
        }
    }

    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Checkpoint")
        {
            target += 1;
            //print(target);
        }
        else if (other.tag == "Finish")
        {
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.isWaveOver();
            GameManager.Instance.UnregisterEnemy(this);
            //Destroy(gameObject);
        }
        else if (other.tag == "Projectile")
        {
            Projectile newP = other.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackStr);
            Destroy(other.gameObject);
        }
    }

    public void EnemyHit(int hitpoints)
    {
        healthPoints -= hitpoints;
        anim.Play("Hurt");
        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
        if(healthPoints < 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(!isDead)
        {

            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
            GameManager.Instance.TotalKilled += 1;
            GameManager.Instance.isWaveOver();
            GameManager.Instance.AddMoney(rewardAmmount);

        }   

        isDead = true;
        anim.SetTrigger("didDie");
         
    }


    
}



