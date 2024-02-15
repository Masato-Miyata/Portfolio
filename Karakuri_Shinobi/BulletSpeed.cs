using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeed : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 1;
    public float MoveSpeed = 20.0f;

    int frameCount = 0;             // フレームカウント
    const int deleteFrame = 1000;    // 削除フレーム

    private float enemyRotation;
    private int attackNumber = 3;
    private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy");

        enemyRotation = enemy.transform.eulerAngles.y;
        Debug.Log(enemyRotation);

    }

    // Update is called once per frame
    void Update()
    {
        if(enemyRotation >= 180)
        {
            transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0);
        }
        else if(enemyRotation <180)
        {
            transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0);
        }
        

        if (++frameCount > deleteFrame)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<BeDamaged>().BeDamage(bulletDamage,attackNumber);
            Debug.Log("aaa");
            Destroy(gameObject);
        }
    }

}