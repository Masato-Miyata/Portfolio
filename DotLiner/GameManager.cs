//using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    GameObject CutIn;
    [SerializeField]
    Text CutInText;

    public static bool stopInputKey = false;

    public static bool player1Trun = true; //どちらが攻撃しているかを保存しておくフィールド

    public static string nextScene = default;

    [Header("アイテム生成なし")]
    public static bool NotItemCreate = false;

    public bool nowWait = true;

    //田中加筆
    [Header("\nプレイヤー(赤色)")]
    [SerializeField]
    GameObject player1;
    //最大塗り回数
    private int redMaxMoveCounter = 3;
    private int nextRedMaxMoveCounter = default;
    public RedPlayerManager redPlayerManager;

    //攻撃した次のターン　塗りポイント半減　切り下げ
    public static bool isRedAttack = false;

    //プレイヤー1のHP
    public int redHp = 2;

    [Header("\nプレイヤー(青色)")]
    [SerializeField]
    GameObject player2;
    //最大塗り回数
    private int blueMaxMoveCounter = 3;
    private int nextBlueMaxMoveCounter = default;
    public BluePlayerManager bluePlayerManager;

    //攻撃した次のターン　塗りポイント半減　切り下げ
    public static bool isBlueAttack = false;

    //プレイヤー2のHP
    public int blueHp = 2;


    [Header("\nポーション所持数")]
    //↓↓↓↓↓横山追記
    //ポーションの最大所持数
    public int portion_limit = 6;
    //ポーションの所持数
    public int portion_red = 0;
    public int portion_blue = 0;
    //行動回数アップ変数
    private int move_up;

    [Header("\nバケツの最大所持数")]
    //バケツの最大所持数
    public int RePaint_limit = 10;
    //↑↑↑↑↑
    //バケツ所持数
    public int redRePaint = 0;
    public int blueRePaint = 0;
    public bool oneTime = false;


    //デバッグ専用(ピッタリゴールしなくてよい)
    public static bool notPerfectGoal = false;
    [Header("デバッグ専用(アタッチしなくてもよい)")]
    [SerializeField]
    GameObject GoalFlag = null;
    // Start is called before the first frame update
    void Start()
    {
        redPlayerManager = player1.GetComponent<RedPlayerManager>();
        bluePlayerManager = player2.GetComponent<BluePlayerManager>();

        animator = CutIn.GetComponent<Animator>();

        nextRedMaxMoveCounter = redMaxMoveCounter;
        nextBlueMaxMoveCounter = blueMaxMoveCounter;

        stopInputKey = false;
        nextScene = default;
        nowWait = true;
        isRedAttack = false;
        redHp = 2;
        isBlueAttack = false;
        blueHp = 2;
        oneTime = false;
        

        //↓↓↓↓↓横山追記
        //初期化
        redRePaint = 0;
        blueRePaint = 0;
        portion_red = 0;
        portion_blue = 0;
        //↑↑↑↑↑

        //菊地加筆
        PPDisplay.ppDisplay.PointDisplay1(redMaxMoveCounter);
        PPDisplay.ppDisplay.PointDisplay2(blueMaxMoveCounter);
    }

    // Update is called once per frame
    void Update()
    {

        if (player1Trun)
        {
            CutInText.color = new Color32(255, 122, 0, 255);
            if (oneTime == false)
            {
                oneTime = true;

                // プレイヤー１の攻撃
                //playerObj1.GetComponent<Player1>().Attack();//プレイヤー1の行動を呼び出す

                Debug.Log("<color=orange> TrunChange! </color>");

                redPlayerManager.PlayertrunUpdate(player1Trun, nextRedMaxMoveCounter, redRePaint);
            }
        }
        else
        {
            CutInText.color = new Color32(0, 122, 255, 255);
            if (oneTime == false)
            {
                oneTime = true;

                // プレイヤー２の攻撃
                Debug.Log("<color=cyan> TrunChange! </color>");
                //playerObj2.GetComponent<Player2>().Attack();

                bluePlayerManager.PlayertrunUpdate(!player1Trun, nextBlueMaxMoveCounter, blueRePaint);
            }
        }
        if (tutorialManager.tutorialNow)
        {
            if (!player1Trun) trunChange();
        }
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.F1))
        {
            trunChange();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            redHp--;
            Debug.Log("redHp" + redHp);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            blueHp--;
            Debug.Log("blueHp" + blueHp);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (notPerfectGoal)
            {
                notPerfectGoal = false;
                if(GoalFlag != null) GoalFlag.SetActive(notPerfectGoal);
            }
            else
            {
                notPerfectGoal = true;
                if (GoalFlag != null) GoalFlag.SetActive(notPerfectGoal);
            }
        }
    }

    //ターンチェンジ
    public void trunChange()
    {

        if (player1Trun)
        {
            StartCoroutine(CutInAnimator());

            //横山追記
            redMaxMoveCounter = 3;
            move_up = redMaxMoveCounter + portion_red;
            redMaxMoveCounter = move_up;

            nextRedMaxMoveCounter = redMaxMoveCounter;

            //塗りポイント半減　切り下げ
            if (isRedAttack)
            {
                if (tutorialManager.tutorialNow && !tutorialManager.launch04)
                {
                    tutorialManager.launch04 = true;
                }

                nextRedMaxMoveCounter = (int)Mathf.Floor(nextRedMaxMoveCounter / 2);
                isRedAttack = false;
            }

            //菊地加筆
            PPDisplay.ppDisplay.PointDisplay1(nextRedMaxMoveCounter);//P1側の塗ポイントを更新して表示


            player1Trun = false;
        }
        else
        {
            StartCoroutine(CutInAnimator());

            //横山追記
            blueMaxMoveCounter = 3;
            move_up = blueMaxMoveCounter + portion_blue;
            blueMaxMoveCounter = move_up;

            nextBlueMaxMoveCounter = blueMaxMoveCounter;

            //塗りポイント半減　切り下げ
            if (isBlueAttack)
            {
                nextBlueMaxMoveCounter = (int)Mathf.Floor(nextBlueMaxMoveCounter / 2);
                isBlueAttack = false;
            }

            //菊地加筆
            PPDisplay.ppDisplay.PointDisplay2(nextBlueMaxMoveCounter);//P2側の塗ポイントを更新して表示

            player1Trun = true;

            //菊地加筆
            PPDisplay.ppDisplay.TurnDisplay();
        }
        oneTime = false;
    }

    //ポーションをとった処理
    public void addMoveCounter()
    {
        //横山追記
        //P1側の処理
        if (player1Trun)
        {
            if (portion_limit > portion_red)
            {
                portion_red++;
            }
        }
        //P2側の処理
        else
        {
            if (portion_limit > portion_blue)
            {
                portion_blue++;
            }
        }
    }
    public void subMoveCounter()
    {
        //P1側の処理
        if (player1Trun)
        {
            if (portion_blue > 0)
            {
                portion_blue--;
            }
        }
        //P2側の処理
        else
        {
            if (portion_red > 0)
            {
                portion_red--;
            }
        }
    }
    //バケツをとった処理
    public void addRePaint()
    {
        //横山追記
        //P1側の処理
        if (player1Trun)
        {
            if (RePaint_limit >= redRePaint)
            {
                //バケツの所持数が９じゃなかったら
                if (redRePaint != 9)
                {
                    redRePaint += 2;
                }
                else
                {
                    redRePaint++;
                }
            }
        }
        //P2側の処理
        else
        {
            if (RePaint_limit >= blueRePaint)
            {
                //バケツの所持数が９じゃなかったら
                if (blueRePaint != 9)
                {
                    blueRePaint += 2;
                }
                else
                {
                    blueRePaint++;

                }
            }
        }
    }
    private IEnumerator CutInAnimator()
    {
        stopInputKey = true;
        animator.SetBool("TrunChange", true);
        yield return new WaitForSecondsRealtime(2.0f);
        animator.SetBool("TrunChange", false);
        stopInputKey = false;
    }

    /// <summary>
    /// 次のシーンに移動する猶予時間
    /// 演出などもここで行う
    /// </summary>
    public void sceneLoadtime()
    {
        stopInputKey = true;
        Invoke("ChangeScene", 1.0f);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
        BGMManager.Instance.PlayBGM(BGMManager.BGM_TYPE.END, 0.6f);
    }
}
