using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TowerManager : Singleton<TowerManager>
{
    private List<Tower> TowerList = new List<Tower>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D BuildTile; 

    private TowerBtn towBtnPress;
    private SpriteRenderer spriteRenderer;

    public TowerBtn TowerBtnPress 
    {
        get
        {
            return towBtnPress;
        }
        set
        {
            towBtnPress = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        BuildTile = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if(hit.collider.tag == "Build Site")
            {
                placeTower(hit);
                
                
            }
        }
        if(spriteRenderer.enabled)
        {
            followMouse();
        }   
    }

    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RegisterTower(Tower tower)
    {
        TowerList.Add(tower);
    }

    public void RenameTagsBuildSite()
    {
        foreach(Collider2D buidTag in BuildList)
        {
            buidTag.tag = "Build Site";

        }
        BuildList.Clear();
    }

    public void DestoryAllTowers()
    {
        foreach(Tower tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }

    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDrag(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }
    public void dissableDrag()
    {
        spriteRenderer.enabled = false;
    }
    public void placeTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towBtnPress != null)
        {
            //Debug.Log("Placed:" + towBtnPress.TowerObject);
            Tower newTower = Instantiate(towBtnPress.TowerObject);
            newTower.transform.position = hit.transform.position;
            hit.collider.tag = "Build Site Full";
            RegisterBuildSite(hit.collider);
            buyTower(towBtnPress.TowerPrice);
            RegisterTower(newTower);
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
            dissableDrag();
        }
    }

    public void buyTower(int price)
    {
        GameManager.Instance.SubMoney(price);
    }

    public void selectedTower(TowerBtn TowerBtnSelected)
    {
        if(TowerBtnSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towBtnPress = TowerBtnSelected;
            enableDrag(towBtnPress.DragSprite);
            
        }
    }
}
