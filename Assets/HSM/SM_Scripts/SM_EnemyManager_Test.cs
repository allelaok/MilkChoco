using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_EnemyManager_Test : MonoBehaviour
{
    //���Ͱ� ������ ��ġ�� ���� �迭
    public Transform[] points;
    //���� �������� �Ҵ��� ����\
    public GameObject monsterPrefab;

    //���͸� �߻���ų �ֱ�
    public float createTime;
    //������ �ִ� �߻� ����
    public int maxMonster = 6;
    //���� ���� ���� ����
    public bool isGameOver = false;

    // Use this for initialization
    void Start()
    {
        //Hierarchy View�� Spawn Point�� ã�� ������ �ִ� ��� Transform ������Ʈ�� ã�ƿ�
        points = GameObject.Find("SM_EnemyManager").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            //���� ���� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(this.CreateMonster());
        }
    }

    IEnumerator CreateMonster()
    {
        //���� ���� �ñ��� ���� ����
        while (!isGameOver)
        {
            //���� ������ ���� ���� ����
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (monsterCount < maxMonster)
            {
                //������ ���� �ֱ� �ð���ŭ ���
                yield return new WaitForSeconds(createTime);

                //�ұ�Ģ���� ��ġ ����
                int idx = Random.Range(1, points.Length);
                //������ ���� ����
                Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }
}

