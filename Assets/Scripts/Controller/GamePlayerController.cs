using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamePlayerController : MonoBehaviour
{
    #region ÇÊ¿äÇÑ º¯¼ö
    public int Count = 0;
    public int BeanCount = 0;
    private bool moveDirect = false; // true = ¿Þ , false = ¿À
    private bool isJump = false; // Á¡ÇÁÁß?
    private bool isImmortal = false;
    private bool isRocket = false;
    private bool isGround = true;
    private int FallCount = 0;
    private bool fallFlag = false;

    [SerializeField]
    private float MoveSpeed = 0.01f;
    [SerializeField]
    private float JumpSpeed = 5f;
    [SerializeField]
    private TextMeshProUGUI Score;
    [SerializeField]
    Transform Character;
    [SerializeField]
    Transform Rocket;
    [SerializeField]
    private float timeOfImmortality = 2f;
    [SerializeField]
    private Transform BGloc;
    #endregion

    #region ÇÊ¿äÇÑ ÄÄÆ÷³ÍÆ®
    private Animator ani;
    [SerializeField]
    private FloorController floorController;
    [SerializeField]
    private EnemyController enemyController;
    [SerializeField]
    public Button JumpButton;
    [SerializeField]
    private TextMeshProUGUI beanCountText;
    #endregion

    //¸ñ¼û
    [SerializeField]
    List<GameObject> heartList = new List<GameObject>();
    int heartIndex = 2;
    int ScoreCount = 5;
    bool CollBoder = false;
    bool die = false;
    bool doubleJump = false;

    StartData _startData;

    void Awake()
    {
        _startData = Managers.Data.Start;
        JumpButton.interactable = false;
        beanCountText.text = Managers.Game.BeanCount.ToString();
        ani = Character.GetComponent<Animator>();
        foreach (CharacterData characterData in Managers.Data.Characters.Values) {
            if (Managers.Game.Character == characterData.ID) {
                ani.gameObject.GetOrAddComponent<BaseController>().SetAnim(characterData.icon2);
                break;
            }
        }
        if (Managers.Game.Character == 1)
        {
            Count = 500;
            BGloc.position = new Vector3(0f,2.3f,0f);
        }
        else if (Managers.Game.Character == 2)
        {
            Count = 2000;
            BGloc.position = new Vector3(0f, -2.35f, 0f);
        }
        else if (Managers.Game.Character == 3)
        {
            heartList[3].SetActive(true);
            heartIndex = 3;
        }
        Score.text = Count.ToString();
        Screen.SetResolution(1080, 1920, true);
        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
    }

    void Update()
    {
        Move();
        DetectKeyDown();
        if (Count >= 2000)
            enemyController.Level = 3;
        else if (Count >= 1200)
            enemyController.Level = 2;
        else if (Count >= 500)
            enemyController.Level = 1;
        else
            enemyController.Level = 0;
    }

    private void DetectKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Menu))
        {
            StopGame();
        }
    }

    public void NormalStart()
    {
        ani.SetBool("Rocket", true); // true => ·ÎÄÏ ²¨Áü
        JumpButton.interactable = true;
    }
    public void StopGame()
    {
        Managers.UI.ShowPopupUI<UI_PausePopup>();
        Time.timeScale = 0;
    }
    private void Move()
    {
        if (!die && !isRocket)
        {
            OriginMove();
            JumpMove();
        }
    }
    public void Jump()
    {
        if (!die && !isJump && !isRocket && isGround && !fallFlag && JumpButton.interactable)
        {
            JumpButton.interactable = false;
            FallCount = 0;
            StartCoroutine(JumpCoroutine());
        }
    }
    private void OriginMove()
    {
        if (!isJump && !isRocket && isGround)
        {
            if (moveDirect)
            {
                Character.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(new Vector3(-0.1f, 0, 0) * Time.deltaTime * MoveSpeed);
            }
            else
            {
                Character.rotation = Quaternion.Euler(0, 180, 0);
                transform.Translate(new Vector3(0.1f, 0, 0) * Time.deltaTime * MoveSpeed);
            }
        }
    }
    private void JumpMove()
    {
        if (isJump && !isRocket && !doubleJump)
        {
            if (moveDirect)
            {
                Character.rotation = Quaternion.Euler(0, 180, 0);
                transform.Translate(new Vector3(-0.1f, 0, 0) * Time.deltaTime * 20f);
            }
            else
            {
                Character.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(new Vector3(0.1f, 0, 0) * Time.deltaTime * 20f);
            }
        }
    }
    private void FallMove()
    {
        if (isJump && !isRocket && !isGround)
        {
            if (moveDirect)
            {
                Character.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(new Vector3(-0.1f, 0, 0) * Time.deltaTime * 20f);
            }
            else
            {
                Character.rotation = Quaternion.Euler(0, 180, 0);
                transform.Translate(new Vector3(0.1f, 0, 0) * Time.deltaTime * 20f);
            }
        }
    }
    public void RocketStart()
    {
        isRocket = true;
        StartCoroutine(RocketCoroutine());
    }
    IEnumerator RocketCoroutine()
    {
        Rocket.gameObject.SetActive(true);
        floorController.StartRocket();
        int last = Count + 250;

        while (Count < last )
        {
            Count += 2;
            Score.text = Count.ToString() + "M";
            yield return new WaitForSeconds(0.0001f);
        }
        yield return new WaitForSeconds(0.3f);

        ani.SetBool("Rocket", true);
        Rocket.gameObject.SetActive(false);
        isRocket = false;
        yield return new WaitForSeconds(0.5f);
        JumpButton.interactable = true;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            TouchBorder();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
            TouchBorder();

        if (other.CompareTag("Thorn")) {
            Managers.Game.TouchThornCount++;
            TouchEnemey();
        }

        if (other.CompareTag("Bird") || other.CompareTag("Monster"))
            TouchEnemey();

        if (other.CompareTag("Ground") && !die)
            TouchGround();

        if (other.CompareTag("Ice") && !die)
            TouchIceBlock();

        if (other.CompareTag("Bean") || other.CompareTag("GoldBean"))
            TouchBean(other);

        //if(other.CompareTag("NoBlock"))
          //  TouchNoBlock();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Gravity") && !isJump && !die)
            TouchGravityBlock();
        if (other.CompareTag("NoBlock") && !fallFlag && !isJump && !isImmortal)
        {
            isGround = false;
            fallFlag = true; 
            TouchNoBlock();
        }
        if (other.CompareTag("NoBlock") && fallFlag && FallCount == 1)
        {
            StartCoroutine(DoubleFall());
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Gravity") && !isJump)
        {
            doubleJump = false;
            floorController.doubleJump = 2f;
            ScoreCount = 5;
        }

        if (other.CompareTag("Ice"))
        {
            MoveSpeed /= 1.5f;
            ani.SetBool("Slide", false);
        }
    }
    private void TouchBorder()
    {
        CollBoder = true;
        moveDirect = !moveDirect;
        Invoke("collBoder", 0.2f);
    }
    private void collBoder()
    {
        CollBoder = false;
    }
    private void TouchEnemey()
    {
        if (!isImmortal && !isRocket)
        {
            if (Managers.Game.Vibration)
                Handheld.Vibrate();
            Managers.Sound.Play(Define.Sound.Effect, "Sound_Hit");
            StartCoroutine(ImmortalCoroutine());
            if (heartIndex == 0)
            {
                DieFunc();
            }
            else
            {
                heartList[heartIndex].SetActive(false);
                heartIndex--;
            }
        }
    }
    private void TouchGround()
    {
        Invoke("OnGround",0.1f);
        ani.SetBool("Fall", false);
    }
    private void OnGround()
    {
        isGround = true;
    }
    private void TouchIceBlock()
    {
        OnGround();
        MoveSpeed *= 1.5f;
        ani.SetBool("Slide", true);
    }
    private void TouchGravityBlock()
    {
        OnGround();
        doubleJump = true;
        floorController.doubleJump = 4f;
        ScoreCount = 10;
    }
    private void TouchNoBlock()
    {
            FallCount++;
            if (FallCount == 2)
            {
                StopAllCoroutines();
                StartCoroutine(FallDieCoroutine());
                return;
            }

            ani.SetBool("Fall", true);
            floorController.MoveUpFloor();
            StartCoroutine(FallCoroutine());
    }
    private void TouchBean(Collider2D other)
    {
        Destroy(other.gameObject);
        if (other.CompareTag("Bean"))
        {
            BeanCount++;
            Managers.Game.BeanCount++;
            Managers.Sound.Play(Define.Sound.Effect, "Sound_EatGreenBean");
        }
        else
        {
            BeanCount += 10;
            Managers.Game.BeanCount += 10;
            Managers.Sound.Play(Define.Sound.Effect, "Sound_EatGoldBean");
        }
        beanCountText.text = Managers.Game.BeanCount.ToString();
    }
    private void DieFunc()
    {
        die = true;
        JumpButton.interactable = false;
        StopAllCoroutines();
        heartList[heartIndex].SetActive(false);
        Managers.UI.ShowPopupUI<UI_QuestionAgainPopup>();
        if (!Managers.Game.NoAds)
        {
            FindObjectOfType<CsStopController>().isAds = true;
            Managers.Ads.ShowInterstitialAds();
        }

    }
    IEnumerator JumpCoroutine()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Jump");
        ani.SetTrigger("Jump");
        isJump = true;
        floorController.MoveFloor();

        if (ScoreCount == 10)
            Managers.Game.RideWindCount++;

        for (int i = 0; i < ScoreCount; i++)
        {
            Count++;
            Score.text = Count.ToString() + "M";
            if (ScoreCount == 5)
                yield return new WaitForSeconds(0.12f);
            else
                yield return new WaitForSeconds(0.06f);
        }

        yield return new WaitForSeconds(0.05f);

        if (!CollBoder)
            moveDirect = !moveDirect;

        doubleJump = false;
        floorController.doubleJump = 2f;
        ScoreCount = 5;
        isJump = false;
        JumpButton.interactable = true;
    }
    IEnumerator FallCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            Count--;
            Score.text = Count.ToString() + "M";
            yield return new WaitForSeconds(0.06f);
        }
        fallFlag = false;
    }
    IEnumerator FallDieCoroutine()
    {
        die = true;
        for (int i = 0; i < 50; i++){
            transform.Translate(new Vector3(0, -0.1f, 0) * Time.deltaTime * 50f);
            yield return null;
        }
        DieFunc();
    }
    IEnumerator ImmortalCoroutine()
    {
        isImmortal = true;
        int count = 0;
        while (count < timeOfImmortality)
        {
            for (int i = 0; i < 5; i++)
            {
                Character.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                yield return new WaitForSeconds(0.1f);
                Character.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                yield return new WaitForSeconds(0.1f);
                count++;
            }
            
        }
        isImmortal = false;
    }    
    IEnumerator DoubleFall()
    {
        yield return new WaitForSeconds(0.34f);
        if (fallFlag)
        {
            StopCoroutine(FallCoroutine());
            fallFlag = true;
            StartCoroutine(FallDieCoroutine());
        }
    }
    public void Revive()
    {
        StopAllCoroutines();
        StartCoroutine(DelayRevive());
    }

    IEnumerator DelayRevive()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Managers.Game.UseRevive = true;
        Managers.Game.ReviveCount++;
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_BGM", 0.5f);

        for (int i = 0; i < 3; i++)
        {
            heartList[i].SetActive(true);
        }
        heartIndex = 2;

        if (Managers.Game.Character == 3)
        {
            heartList[3].SetActive(true);
            heartIndex = 3;
        }

        if (transform.position.x <= 0)
            moveDirect = false;
        else
            moveDirect = true;

        transform.position = new Vector3(0, 0.2f, 0);
        Time.timeScale = 1f;
        JumpButton.interactable = true;
        isGround = true;
        isJump = false;
        die = false;
        fallFlag = false;
        isRocket = false;
        CollBoder = false;
        isImmortal = false;
        FallCount = 0;
        StartCoroutine(ImmortalCoroutine());
    }
}
/*
 *     private bool isJump = false; // Á¡ÇÁÁß?
    private bool isImmortal = false;
    private bool isRocket = false;
    private bool isGround = true;
    private int FallCount = 0;
    private bool fallFlag = false;
 */