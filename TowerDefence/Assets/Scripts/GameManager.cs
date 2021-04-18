using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gameStatus
{
    next, play, gameOver, win
}

public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private UnityEngine.UI.Button playBtn;
    [SerializeField]
    private UnityEngine.UI.Text playBtnLbl;
    [SerializeField]
    private UnityEngine.UI.Text totalMoneyLbl;
    [SerializeField]
    private UnityEngine.UI.Text currentWaveLbl;
    [SerializeField]
    private UnityEngine.UI.Text totalEscapedLbl;


    [SerializeField]
    private int totalWaves;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private Enemy[] enemies;
    [SerializeField]
    private int totalEnemies = 3;
    [SerializeField]
    private int enemiesPerSpawn;
    
    private int waveNo = 0;
    private int totalMoney = 15;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int which2Spawn = 0;
    private gameStatus currentState = gameStatus.play;
    private AudioSource audioSource; 

    //private int enemiesOnScreen = 0;
    const float spawnDelay = 0.5f;
    public List<Enemy> EnemyList = new List<Enemy>();

    public int TotalMoney 
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLbl.text = totalMoney.ToString();
        }
    }

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set 
        {
            totalKilled = value; 
        }
    }

    public AudioSource AudioSource
    {
        get
        {
           return audioSource;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
       //StartCoroutine(spwanEnemy());
    }

    void Update()
    {
        HandleEscp();
    }

    IEnumerator spwanEnemy()
    {
        if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            
            for(int i=0; i < enemiesPerSpawn; i++)
            {
                
                if(EnemyList.Count < totalEnemies)
                {
                    
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0,which2Spawn)]);
                    newEnemy.transform.position = spawnPoint.transform.position;
                    //print(enemiesOnScreen);
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(spwanEnemy());
        }
    }
/*
    public void removeEnemyFromScreen()
    {
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen -= 1;
        }
    }
*/
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int ammount)
    {
        TotalMoney += ammount;
    }

    public void SubMoney(int ammount)
    {
        TotalMoney -= ammount;
    }

    public void ShowMenu()
    {
        switch(currentState)
        {
            case gameStatus.gameOver :
                playBtnLbl.text = "Play Again?";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
                break;
            case gameStatus.next : 
                playBtnLbl.text = "Next Wave";
                break;
            case gameStatus.play : 
                playBtnLbl.text = "Play";
                break;
            case gameStatus.win :
                playBtnLbl.text = "Play";
                break;
        }

        playBtn.gameObject.SetActive(true);
    }

    private void HandleEscp()
    {
        if(Input.GetMouseButtonDown(1))
        {
            TowerManager.Instance.dissableDrag();
            TowerManager.Instance.TowerBtnPress = null;

        }
    }

    public void isWaveOver()
    {
        totalEscapedLbl.text = "Escaped " + TotalEscaped.ToString() + "/10";
        if((roundEscaped + totalKilled) >= totalEnemies)
        {
            if(waveNo <= enemies.Length)
            {
                which2Spawn = waveNo;
            }
            print("wave Over");
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState()
    {
        if(TotalEscaped >= 10)
        {
            currentState = gameStatus.gameOver;
            //print("gameover");
        }
        else if(waveNo == 0 && (TotalKilled + roundEscaped) == 0)
        {
            currentState = gameStatus.play;
            //print("play");
        }
        else if(waveNo >= totalWaves)
        {
            currentState = gameStatus.win;
            //print("win");
        }
        else
        {
            currentState = gameStatus.next;
            //print("next");
        }
    }
    
    public void PlayButtonPress()
    {
        switch(currentState)
        {
            case gameStatus.next:
            waveNo += 1;
            totalEnemies += waveNo;
            AudioSource.PlayOneShot(SoundManager.Instance.NewGame);
            break;

            default: 
            totalEnemies = 3; 
            TotalEscaped = 0;
            roundEscaped = 0;
            TotalMoney = 10;
            which2Spawn = 0;
            totalMoneyLbl.text = TotalMoney.ToString();
            totalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
            TowerManager.Instance.DestoryAllTowers();
            TowerManager.Instance.RenameTagsBuildSite();
            audioSource.PlayOneShot(SoundManager.Instance.NewGame);
            break;
        }
    

    DestroyAllEnemies();
    TotalKilled = 0;
    roundEscaped = 0;
    currentWaveLbl.text = "Wave " + (waveNo+1);
    StartCoroutine(spwanEnemy());
    playBtn.gameObject.SetActive(false);

    }

}  



