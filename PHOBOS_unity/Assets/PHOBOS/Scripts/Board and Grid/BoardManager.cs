using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public List<Sprite> characters = new List<Sprite>();
    
    public GameObject tile;

    public Text DamegeTxt;

    public int xSize, ySize;

    private GameObject[,] tiles;
    private GameObject GUI_obj;

    public bool IsShifting { get; set; }

    void Start()
    {
        instance = GetComponent<BoardManager>();
        GUI_obj = GameObject.Find("GUIManagerCanvas");

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }



    private void CreateBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[xSize, ySize];

        float startX = transform.position.x;
        float startY = transform.position.y;

        Sprite[] previousLeft = new Sprite[ySize];
        Sprite previousBelow = null;


        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
                tiles[x, y] = newTile; //x, y위치에 타일 생성

                newTile.transform.parent = transform; // 1
                List<Sprite> possibleCharacters = new List<Sprite>(); // 1
                possibleCharacters.AddRange(characters); // 2

                possibleCharacters.Remove(previousLeft[y]); // 3
                possibleCharacters.Remove(previousBelow);

                Sprite newSprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite; // 3

                previousLeft[y] = newSprite;
                previousBelow = newSprite;

            }
        }
    }

    public IEnumerator FindNullTiles()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
                }
            }
        }
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                tiles[x, y].GetComponent<Tile>().ClearAllMatches();
            }
        }

    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .05f)
    { //빈 공간만큼 패널을 밑으로 내리는 함수
        IsShifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0; //공백 수를 세는 변수

        for (int y = yStart; y < ySize; y++)
        {
            SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null)
            { // 아래쪽에 공간이 있다면 nullCount를 하나 추가
                nullCount++;
            }
            renders.Add(render);
        }

        for (int i = 0; i < nullCount; i++)
        {
            //            GUIManager.instance.Score += 50; //패널 1개당 50점 추가
            GUIManager.instance.EnemyHP -= 50; //score 만큼 HP가 깎임
            //Instantiate(DamegeTxt, new Vector3(1130.9f, 150f, 0), Quaternion.identity).transform.parent = GUI_obj.transform;
            
            //DamegeTxt.text = "-50";
            //Destroy(DamegeTxt, 0.5f);
            GUIManager.instance.PlayerMP += 10; //playerMP 회복
            Debug.Log(GUIManager.instance.BSanchipinchi);
            if (GUIManager.instance.PlayerSAN < GUIManager.instance.playerMaxSAN)
            {
                if (GUIManager.instance.BSanchipinchi) //BSanchipinchi가 true일 경우 SAN을 더 많이 회복함
                {
                    Debug.Log("산치핀치 회복");
                    GUIManager.instance.PlayerSAN += 50;
                }
                else
                {
                    GUIManager.instance.PlayerSAN += 2;
                }
            }

            yield return new WaitForSeconds(shiftDelay); //shiftDelay 만큼 딜레이를 줌 
            for (int k = 0; k < renders.Count - 1; k++)
            { // 5
                renders[k].sprite = renders[k + 1].sprite;
                renders[k + 1].sprite = null; // 6 
                renders[k + 1].sprite = GetNewSprite(x, ySize - 1);
            }
            IsShifting = false;
        }
    }

    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        if (x > 0)
        {
            possibleCharacters.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < xSize - 1)
        {
            possibleCharacters.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleCharacters.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
    }
}


