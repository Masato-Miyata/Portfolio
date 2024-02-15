using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public int Maxhp = 30; 
    [SerializeField] private float decreaseSpan;
    [SerializeField] private int timeCost = 1;
    public int Currenthp;

    [SerializeField] private GameObject player; //プレイヤーのHPバー
    [SerializeField] private Slider playerHp; //プレイヤーのHPバー

    [SerializeField]private float invincibleTime = 0.5f;
    [SerializeField]
    private float blinkingCycle = 0.1f;
    private bool isBlinking = false;

    [HideInInspector]public bool Isinvincivle = false;

    private SpriteRenderer sr = null;




    void Start()
    {
        sr = player.GetComponent<SpriteRenderer>();
        StartCoroutine("HPDecrease");
        Currenthp = Maxhp;
        Debug.Log("�ő�HP" + Currenthp);

        playerHp.maxValue = Currenthp;
        playerHp.value = Currenthp;
    }

    public IEnumerator HPDecrease()
    {

        while (true)
        {
            yield return new WaitForSeconds(decreaseSpan);
            Currenthp -= timeCost;
            playerHp.value = Currenthp;
            Debug.Log(Currenthp);
        }
    }

    public void AttackCost(int cost) //PlayerAttackBox.csから引数でダメージの値を受け取っている。
    {
        Currenthp -= cost;
        playerHp.value = Currenthp;
        Debug.Log("AttackCost");
    }

    public void DashCost(int dashCost) //PlayerAttackBox.csから引数でダメージの値を受け取っている。
    {
        Currenthp -= dashCost;
        playerHp.value = Currenthp;
        Debug.Log("DashCost");
    }

    public void BeDamage(int beDamage)//BeDamaged.csから引数でダメージの値を受け取っている。
    {
        isBlinking = true;
        Currenthp -= beDamage;
        Debug.Log("BeDamage");
        playerHp.value = Currenthp;
        StartCoroutine("BlinkingTime");
        StartCoroutine("InvincibleTime");
    }

    public void HPRecovery(int healValue, bool isMax)//BeDamaged.csから引数でダメージの値を受け取っている。
    {
        if(isMax == false)
        {
            Currenthp += healValue;
            Debug.Log("Heal");
            playerHp.value = Currenthp;
        }
        else
        {
            Currenthp = Maxhp;
            Debug.Log("MaxHP");
            playerHp.value = Currenthp;
        }
        
    }

    void Update()
    {
        // if (Currenthp <= 0)
        // {
        //     GManager.instance.GameOver();
        //     Currenthp = 0;
        // }

    }

    public IEnumerator InvincibleTime()//BeDamaged.csで敵から接触攻撃を受けた際にここの無敵時間のコルーチンが呼ばれている。
    {
        yield return new WaitForSeconds(invincibleTime);
        Debug.Log("無敵終了");
        isBlinking = false;
        sr.material.color = Color.white;
        Isinvincivle = false;

    }

    public IEnumerator BlinkingTime()//BeDamaged.csで敵から接触攻撃を受けた際にここの無敵時間のコルーチンが呼ばれている。
    {
        while (isBlinking)
        {
            sr.material.color = Color.black;
            yield return new WaitForSeconds(blinkingCycle);
            sr.material.color = Color.white;
            yield return new WaitForSeconds(blinkingCycle);
        }
        

    }
}
