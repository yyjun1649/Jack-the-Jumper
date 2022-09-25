using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private List<Vector2> Pivot = new List<Vector2>(); // 0 ~ 2 ©Л , 3~ 5 аб

    [SerializeField]
    private Transform PivotGroup;

    [SerializeField]
    private List<GameObject> RightEnemey = new List<GameObject>();
    [SerializeField]
    private List<GameObject> LeftEnemey = new List<GameObject>();

    [HideInInspector]
    public int Level = 0;

    int EnemyLevel = 0;
    float EnemeySpeed = 9f;
    int AppearCount = 500;
    public bool pause = false;

    string Controller = "Plane";

    void Start()
    {
        StartCoroutine(TimeCoroutine());
    }

    void Update()
    {
        if (Level == 1)
        {
            EnemeySpeed = 10.35f;
            AppearCount = 450;
            Controller = "Plane";
            EnemyLevel = 1;
        }
        else if (Level == 2)
        {
            EnemeySpeed = 11.25f;
            AppearCount = 425;
            Controller = "Rock";
            EnemyLevel = 1;
        }
        else if (Level == 3)
        {
            EnemeySpeed = 12.15f;
            AppearCount = 400;
            Controller = "UFO";
            EnemyLevel = 1;
        }
    }

    IEnumerator TimeCoroutine()
    {
        yield return new WaitForSeconds(3f);
        int time = 1;
        while (true)
        {
            time++;
            if (time % AppearCount == 0)
            {
                MakeEnemy();
            }
            yield return null;
        }
    }

    private void MakeEnemy()
    {
        if (!pause)
        {
            int patternRan = Random.Range(0, 6);
            GameObject obj;
            int MakeEnemyColor = Random.Range(0, 3);

            if (patternRan < 3)
                obj = Instantiate(RightEnemey[EnemyLevel]);
            else
                obj = Instantiate(LeftEnemey[EnemyLevel]);


            if (Level >= 1)
                obj.transform.Find("Enemy").Find("Obj").gameObject.GetOrAddComponent<BaseController>().SetAnim("Animation/EnemyAnimation/" + Controller + "/" + "Controller" + MakeEnemyColor.ToString());

            if (Level == 2)
                obj.transform.Find("Enemy").Find("Obj").GetComponent<BoxCollider2D>().size = new Vector2(7.8f, 8f);

            if (Level == 3)
                obj.transform.Find("Enemy").Find("Obj").GetComponent<BoxCollider2D>().size = new Vector2(1.25f, 1.2f);

            obj.transform.position = Pivot[patternRan];
            obj.transform.parent = PivotGroup;

            StartCoroutine(MoveEnemy(obj, patternRan));
        }
    }

    IEnumerator MoveEnemy(GameObject obj1, int ran)
    {
        int count = 0;

        while (count < 4000)
        {
            count++;

            obj1.transform.Translate(new Vector3(-1f, 0, 0) * Time.deltaTime * EnemeySpeed);

            yield return null;
        }
        Destroy(obj1);
    }
}
