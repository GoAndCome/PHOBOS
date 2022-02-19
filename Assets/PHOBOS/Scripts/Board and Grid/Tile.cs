using UnityEngine;
using System.Collections.Generic;

enum spriteColor
{
    RED = 0, //하트 패널
    YELLOW = 1, //주먹 패널
    BLUE = 2, //앙크 패널
    PURPLE = 3 //육망성 패널
}

public class Tile : MonoBehaviour
{
    public static Tile instance;

    public GameObject machingEffect;
    public GameObject selectEffect;

    public short spritevalue;

    private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
    private static Tile previousSelected = null;

    private SpriteRenderer render;
    private bool isSelected = false;

    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private bool matchFound = false;



    // private float delay = 0.08f;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();     
    }

    private void Start()
    {
    }

    private void Update()
    { //spritevalue에 이 패널이 어떤 스프라이트를 가지는지 넣기
      //if (render.sprite.name == "하트패널_수정본")
      //if(GetComponent<SpriteRenderer>().sprite.name == "하트패널_수정본")
        /*if (gameObject.GetComponent<SpriteRenderer>().sprite.name != null) {
            if (spritevalue == 5)
            {
                Debug.Log("스프라이트 이름 : " + gameObject.GetComponent<SpriteRenderer>().sprite.name);
                if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "심장패널")
                {
                    spritevalue = (short)spriteColor.RED; //0
                }
                else if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "해골패널")
                {
                    spritevalue = (short)spriteColor.YELLOW; //1
                }
                else if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "앙크패널")
                {
                    spritevalue = (short)spriteColor.BLUE; //2
                }
                else if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "육망성패널")
                {
                    spritevalue = (short)spriteColor.PURPLE; //3
                } else
                {
                    Debug.Log("다른 값");
                }

                Debug.Log("들어온 값 : " + spritevalue);
            }
        }*/
    }


    private void Select()
    {
        isSelected = true;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z - 0.5f);
        Instantiate(selectEffect, pos, Quaternion.identity);
        render.color = selectedColor;
        previousSelected = gameObject.GetComponent<Tile>();
        SFXManager.instance.PlaySFX(Clip.Select);
    }

    private void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
        previousSelected = null;
    }

    void OnMouseDown()
    {
        if (!GameManager.instance.gameOver)
        {
            Destroy(GameObject.Find("panner_effect(Clone)"));

            if (render.sprite == null || BoardManager.instance.IsShifting)
            {
                return;
            }

            if (isSelected)
            {
                Deselect();
            }
            else
            {
                if (previousSelected == null)
                {
                    Select();
                }
                else
                {
                    if (GetAllAdjacentTiles().Contains(previousSelected.gameObject))
                    {
                        SwapSprite(previousSelected.render); //타일의 스프라이트 바꾸기
                                                             //Invoke("MatchDelay", delay);

                        previousSelected.ClearAllMatches();
                        previousSelected.Deselect();
                        ClearAllMatches();
                        if (GUIManager.instance.ComboCount < 2)
                        {
                            GUIManager.instance.ComboCount = 0;
                        }

                    }
                    else
                    {
                        previousSelected.GetComponent<Tile>().Deselect();
                        Select();
                    }
                }
            }
        }
    }

    public void SwapSprite(SpriteRenderer render2)
    {
        if (render.sprite == render2.sprite)
        {
            return;
        }
        Sprite tempSprite = render2.sprite;
        render2.sprite = render.sprite;
        render.sprite = tempSprite;
        SFXManager.instance.PlaySFX(Clip.Swap); //효과음
        GUIManager.instance.MoveCounter -= 1;
    }

    private GameObject GetAdjacent(Vector2 castDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private List<GameObject> GetAllAdjacentTiles()
    {
        List<GameObject> adjacentTiles = new List<GameObject>(); //현재 타일을 둘러싼 타일 리스트 생성 
        for (int i = 0; i < adjacentDirections.Length; i++) //adjacentDirections는 백터2 1차원 배열 (Vector2.up, Vector2.down, Vector2.left, Vector2.right)
        { //adjacentDirections에 들어가 있는 배열의 개수만큼 for문을 돌림    
            adjacentTiles.Add(GetAdjacent(adjacentDirections[i])); //인접 타일 리스트에 인접 방향 데이터를 (adjacentDirections[i]) 추가
        }
        return adjacentTiles; //리스트 리턴
    }

    private List<GameObject> FindMatch(Vector2 castDir)
    {
        List<GameObject> matchingTiles = new List<GameObject>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);      //castDir 방향으로 레이캐스트를 발사
        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite)
        {                //아무것도 맞지 않거나 자신의 스프라이트와 반환된 스프라이트가 다를 때 까지 레이캐스트 발사 
            matchingTiles.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
        }
        return matchingTiles;
    }
    private void ClearMatch(Vector2[] paths) // 1
    {
        List<GameObject> matchingTiles = new List<GameObject>(); // 2
        for (int i = 0; i < paths.Length; i++) // 3
        {
            matchingTiles.AddRange(FindMatch(paths[i]));
        }
        if (matchingTiles.Count >= 2) // 4
        {
            for (int i = 0; i < matchingTiles.Count; i++) // 5
            {
                Vector3 pos = matchingTiles[i].transform.position;
                pos.z = -1f;
                Instantiate(machingEffect, pos, Quaternion.identity);

                matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
                // 매칭된 횟수에 따라 콤보 넣기
                if (i == matchingTiles.Count - 1)
                {
                    GUIManager.instance.ComboCount += 1;
                }
            }
            matchFound = true; // 6
        }
    }

    public void ClearAllMatches()
    {
        if (render.sprite == null)
        {
            return;
        }
        ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        ClearMatch(new Vector2[2] { Vector2.down, Vector2.up });

        if (matchFound)
        {
            render.sprite = null;
            matchFound = false;
            StopCoroutine(BoardManager.instance.FindNullTiles());
            StartCoroutine(BoardManager.instance.FindNullTiles());
            SFXManager.instance.PlaySFX(Clip.Clear); //효과음
        }
    }

    public void MatchDelay()
    {
        previousSelected.ClearAllMatches();
        previousSelected.Deselect();
        ClearAllMatches();

    }
}