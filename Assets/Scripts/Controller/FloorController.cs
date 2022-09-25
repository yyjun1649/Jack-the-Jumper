using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    bool FlatRot = false;

    [SerializeField]
    private List<GameObject> FloorPrefab = new List<GameObject>(); // 0~ 3 Floor , 4~7 Thorn, 8~11 NoBlock, 12~ 15 Ice, 16~19 Satelite, 20~23 Gravity, 24~27 Space

    [SerializeField]
    List<Vector2> PivotList = new List<Vector2>();

    private Vector3 FloorCurPosition;

    Queue<GameObject> FloorList = new Queue<GameObject>();

    [SerializeField]
    private Transform BackSprite;

    [SerializeField]
    private BeanController beanController;

    [SerializeField]
    private EnemyController enemyController;

    public float doubleJump = 2f;
    float moveSpeed = 3f;
    float RocketPivot = 8f;
    bool Fall = false;
    bool flag = true;
    int FallCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        FloorCurPosition = transform.position;
        FloorInitialize();
    }

    private void FloorInitialize()
    {
        for (int i = 0; i < 5; i++)
        {
            FloorList.Enqueue(CreateFloor(i + 1, true));
        }
    }

    private GameObject CreateFloor(int i, bool bo)
    {
        FlatRot = !FlatRot;
        GameObject newObj;
        if (!bo) {
            if (enemyController.Level == 0)
                newObj = Level0();
            else if (enemyController.Level == 1)
                newObj = Level1();
            else if (enemyController.Level == 2)
                newObj = Level2();
            else
                newObj = Level3();
        }
        else
            newObj = Instantiate(FloorPrefab[Random.Range(0,4)]);


        newObj.transform.position = PivotList[i];
        newObj.transform.parent = transform;
        return newObj;
    }
    private GameObject Level0()
    {
        GameObject newObj;

        int ran = Random.Range(0, 100); // 0 ~ 99 숫자 

        if (ran >= 50)
            newObj = Instantiate(FloorPrefab[Random.Range(0, 4)]); // 장애물 X 발판
        else if (ran>= 25)
            newObj = Instantiate(FloorPrefab[Random.Range(4, 8)]); // 가시 발판
        else
            newObj = Instantiate(FloorPrefab[Random.Range(8, 12)]); // 좌우 비어있는 발판

        return newObj;
    }

    private GameObject Level1()
    {
        GameObject newObj;
        int ran = Random.Range(0, 100);

        if (ran >= 70)
            newObj = Instantiate(FloorPrefab[Random.Range(0, 4)]);
        else if (ran >= 45)
            newObj = Instantiate(FloorPrefab[Random.Range(4, 8)]);
        else if (ran >= 25)
            newObj = Instantiate(FloorPrefab[Random.Range(8, 12)]);
        else if (ran > 15)
            newObj = Instantiate(FloorPrefab[Random.Range(20, 24)]);
        else
            newObj = Instantiate(FloorPrefab[Random.Range(12, 16)]); // 얼음발판

        return newObj;
    }

    private GameObject Level2()
    {
        GameObject newObj;
        int ran = Random.Range(0, 100);

        if (ran >= 85)
            newObj = Instantiate(FloorPrefab[Random.Range(0, 4)]);
        else if (ran >= 50)
            newObj = Instantiate(FloorPrefab[Random.Range(4, 8)]);
        else if (ran >= 35)
            newObj = Instantiate(FloorPrefab[Random.Range(8, 12)]);
        else if (ran >= 25)
            newObj = Instantiate(FloorPrefab[Random.Range(12, 16)]);
        else if (ran>= 15)
            newObj = Instantiate(FloorPrefab[Random.Range(20, 24)]);
        else
            newObj = Instantiate(FloorPrefab[Random.Range(16, 20)]);

        return newObj;
    }

    private GameObject Level3()
    {
        GameObject newObj;
        int ran = Random.Range(0, 100);

        if (ran >= 70)
            newObj = Instantiate(FloorPrefab[Random.Range(0, 4)]);
        else if (ran >= 50)
            newObj = Instantiate(FloorPrefab[Random.Range(4, 8)]);
        else if (ran >= 35)
            newObj = Instantiate(FloorPrefab[Random.Range(8, 12)]);
        else if (ran >= 25)
            newObj = Instantiate(FloorPrefab[Random.Range(12, 16)]);
        else if (ran >= 15)
            newObj = Instantiate(FloorPrefab[Random.Range(20, 24)]);
        else
        {
            Managers.Game.MonsterCount++;
            newObj = Instantiate(FloorPrefab[Random.Range(24, 28)]);
            Managers.Game.SaveGame();
        }

        return newObj;
    }


    private GameObject RocketCreateFloor(int i, bool bo)
    {
        GameObject newObj;
        newObj = Instantiate(FloorPrefab[0]);
        newObj.transform.position = new Vector2(0,RocketPivot);
        RocketPivot += 4f;
        newObj.transform.parent = transform;
        return newObj;
    }

    public void MoveFloor()
    {
        if (!Fall)
        {
            FloorList.Enqueue(CreateFloor(6, false));
            beanController.MakeBean(8.7f);
            if (doubleJump == 4f)
            {
                FloorList.Enqueue(CreateFloor(7, false));
                beanController.MakeBean(10.7f);
            }
        }
        StartCoroutine(JumpCoroutine()); 
    }

    public void MoveUpFloor()
    {
        StartCoroutine(FallCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        if (flag)
        {
            flag = false;

            Vector3 targetPosition = new Vector3(0, transform.position.y - doubleJump, 0);
            FallCount = 0;
            Fall = false;
            if (doubleJump == 4)
                moveSpeed = 1.5f;
            else
                moveSpeed = 3f;

            while (Vector3.Magnitude(targetPosition - transform.position) >= 0.1f)
            {
                transform.Translate(new Vector3(0, -doubleJump, 0) * Time.deltaTime * moveSpeed);
                if(BackSprite.transform.position.y > -2.5)
                    BackSprite.Translate(new Vector3(0, -0.05f, 0) * Time.deltaTime * moveSpeed);
                yield return null;
            }

            transform.position = targetPosition;

            while (FloorList.Count > 5)
            {
                Destroy(FloorList.Dequeue());
            }
            transform.position = targetPosition;

            flag = true;
        }
    }
    private IEnumerator FallCoroutine()
    {
        FallCount++;
        if (FallCount < 2)
        {
            Fall = true;
            Vector3 targetPosition = new Vector3(0, transform.position.y + 2f, 0);

            if (doubleJump == 4)
                moveSpeed = 1.5f;
            else
                moveSpeed = 3f;

            while (Vector3.Magnitude(targetPosition - transform.position) >= 0.1f)
            {
                transform.Translate(new Vector3(0, 2f, 0) * Time.deltaTime * moveSpeed);
                if (BackSprite.transform.position.y < 9)
                    BackSprite.Translate(new Vector3(0, +0.2f, 0) * Time.deltaTime * moveSpeed);
                yield return null;
            }

            transform.position = targetPosition;
        }
    }

    public void StartRocket()
    {
        StartCoroutine(RocketCoroutine());
    }

    IEnumerator RocketCoroutine()
    {
        if (flag)
        {
            flag = false;
            int count = 0;
            Vector3 targetPosition = new Vector3(0, FloorCurPosition.y - 2f, 0);

            while (count < 6)
            {
                while (Vector3.Magnitude(targetPosition - transform.position) >= 0.1f)
                {
                    transform.Translate(new Vector3(0, -2f, 0) * Time.deltaTime * 6f);
                    BackSprite.Translate(new Vector3(0, -0.1f, 0) * Time.deltaTime * 6f);
                    yield return null;
                }


                transform.position = targetPosition;

                while (FloorList.Count > 5)
                {
                    Destroy(FloorList.Dequeue());
                }

                transform.position = new Vector3(0, 2f, 0);

                FloorCurPosition = new Vector3(0, transform.position.y, 0);
                count++;
            }
            transform.position = new Vector3(0, 0f, 0);
            FloorList.Enqueue(CreateFloor(6, false));

            while (Vector3.Magnitude(targetPosition - transform.position) >= 0.1f)
            {
                transform.Translate(new Vector3(0, -1f, 0) * Time.deltaTime * 3f);
                BackSprite.Translate(new Vector3(0, -0.1f, 0) * Time.deltaTime * 3f);
                yield return null;
            }

            transform.position = targetPosition;
            FloorCurPosition = targetPosition;

            while (Vector3.Magnitude(targetPosition - transform.position) >= 0.1f)
            {
                transform.Translate(new Vector3(0, 1f, 0) * Time.deltaTime * 3f);
                BackSprite.Translate(new Vector3(0, 0.1f, 0) * Time.deltaTime * 3f);
                yield return null;
            }
            flag = true;
        }
    }
}
