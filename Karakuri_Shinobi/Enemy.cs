using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool Enemydied = false;
    public int Enemystate = 0;
    public bool Iscloserange = true;
    [SerializeField]
    private int closeRangeAttackDamage = 4;
    private Vector3 MoveX;//横移動速度
    [SerializeField]
    private float walkTime = 2.0f;
    public float moveSpan = 0f;//行動切り替えのスパン
    [SerializeField]
    private float jumpForce = 0f;//ジャンプ力
    private Rigidbody2D rbody2D;
    private Animator anim = null;
    private float jumpAnimTime = 0.3f;
    [SerializeField]
    private float untilAttackTime = 0.5f;
    [SerializeField]
    private float attackAnimTime = 0.3f;
    [SerializeField]
    private float closeAttackAnimTime = 0.27f;
    [SerializeField]
    private float attackSpan = 0.5f;

    private float AttackTime = 0.77f;

    private int attackNumber = 2;

    [SerializeField]
    private GameObject bullet; //遠距離攻撃の弾

    [SerializeField]
    private GameObject bulletPoint;//弾を撃つポイント

    [SerializeField]
    private Transform playerPos;//プレイヤーの方向を向くため。

    [SerializeField]
    private BeDamaged beDamaged;


    // Start is called before the first frame update
    void Start()
    {
        AttackTime = attackSpan + closeAttackAnimTime;
        StartCoroutine(Move());
        anim = GetComponent<Animator>();
        MoveX = new Vector3(0f , 0f , 0f);
        transform.position += MoveX;
        rbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(playerPos.position.x <= this.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0,0,0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0,180,0);
        }
    }

    IEnumerator Move()
    {
        yield return null;
        while (Enemydied == false)
        {   

            if(Enemystate != 5)
            {
                Enemystate =Random.Range(0,4); //5パターンの行動を取る
            }

            switch(Enemystate)
            {

                case 0:
                    StartCoroutine(Advance());
                    yield return new WaitForSeconds(moveSpan);
                    break;
                case 1:
                    StartCoroutine(Recession());
                    yield return new WaitForSeconds(moveSpan);
                    break;
                case 2:
                    StartCoroutine(EnemyJump());
                    yield return new WaitForSeconds(moveSpan);
                    break;
                case 3:
                    StartCoroutine(MediumRangeAttack());
                    yield return new WaitForSeconds(moveSpan);
                    break;
                case 4:
                    StartCoroutine(LongRangeAttack());
                    yield return new WaitForSeconds(moveSpan);
                    break;
                case 5:
                    yield return new WaitForSeconds(untilAttackTime);
                    StartCoroutine("CloseRangeAttack");
                    yield return new WaitForSeconds(AttackTime);
                    break;

                default: //それ以外のdefault

                    Debug.Log("Error");
                    break;
            }
            
        }
        
    }

    public IEnumerator Advance()
    {
        Debug.Log("ヨーソロー");
        float se=0f;
        while(se<=walkTime)
        {
            se+=Time.deltaTime;
            transform.eulerAngles = new Vector3(0,0,0);
            anim.SetBool("Run", true);
            Vector3 MoveX = new Vector3(-5.0f, 0.0f, 0.0f);
            transform.position += MoveX * Time.deltaTime;
            yield return null;
        }
        anim.SetBool("Run", false);
        
    }
    
    public IEnumerator Recession()
    {
        Debug.Log("ぴぎゃ");
        float se=0f;
        while(se<=2.0f)
        {
            se+=Time.deltaTime;
            transform.eulerAngles = new Vector3(0,-180,0);
            anim.SetBool("Run", true);
            Vector3 MoveX = new Vector3(5.0f, 0.0f, 0.0f);
            transform.position += MoveX * Time.deltaTime;
            yield return null;
        }
        anim.SetBool("Run", false);
    }

    public IEnumerator EnemyJump()
    {
        Debug.Log("jumping heart");
        anim.SetBool("Jump", true);
        this.rbody2D.AddForce(transform.up * jumpForce);
        yield return new WaitForSeconds(jumpAnimTime);
        anim.SetBool("Jump", false);
        yield return null;
    }

    public IEnumerator CloseRangeAttack()
    {
        anim.SetBool("CloseRangeAttack", true);
        beDamaged.BeDamage(closeRangeAttackDamage,attackNumber);
        yield return new WaitForSeconds(closeAttackAnimTime);
        anim.SetBool("CloseRangeAttack", false);
         yield return new WaitForSeconds(attackSpan);

    }

    public IEnumerator MediumRangeAttack()
    {
        anim.SetBool("Attack", true);
        Vector3 bulletPos = bulletPoint.transform.position;
        Instantiate(bullet,bulletPos,this.transform.rotation);
        Debug.Log("中距離あたっく");
        yield return new WaitForSeconds(attackAnimTime);
        anim.SetBool("Attack", false);
        yield return null;
    }

    public IEnumerator LongRangeAttack()
    {
        Debug.Log("遠距離あたっく");
        yield return null;
    }
}
