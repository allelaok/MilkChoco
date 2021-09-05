using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���
// ���� �ð��� �ѹ��� ���� ����� �ʹ�.
// �ʿ�Ӽ� : �����ð�, ����ð�, �� ����

// ������ƮǮ�� �̿��Ͽ� ���� �̸� ���� ������ ���� �ʹ�.
// �ʿ�Ӽ� : ������ƮǮ, Ǯũ��

// �����ð��� �ѹ��� ���� SpawnPoints �� ������ְ� �ʹ�.(�������� sp ����)
// �ʿ�Ӽ� : SpawnPoints
public class SM_EnemyManager : MonoBehaviour
{
    // �ʿ�Ӽ� : �����ð�, ����ð�, �� ����
    public float createTime = 2;
    float currentTime = 0;
    public GameObject enemyFactory;

    // �ʿ�Ӽ� : ������ƮǮ, Ǯũ��
    public int enemyPoolSize = 20;
    //public GameObject[] enemyPool;
    //[System.NonSerialized]
    [HideInInspector]
    public List<GameObject> enemyPool = new List<GameObject>();


    // �̱��� ������������ �̿��Ͽ� EnemyManager �� ����ϰ� �ʹ�.
    public static SM_EnemyManager Instance = null;

    // �ʿ�Ӽ� : SpawnPoints
    public Transform[] spawnpoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // �迭�� ũ�⸦ �����ش�.
        //enemyPool = new GameObject[enemyPoolSize];
        // ������ƮǮ�� �̿��Ͽ� ���� �̸� ���� ������ ���� �ʹ�.
        // for(�ʱⰪ����;���ǽ�;������)

        for (int i = 0; i < enemyPoolSize; i++)
        {
            // to do
            // �� ���忡�� ���� ����� �ʹ�.
            GameObject enemy = Instantiate(enemyFactory);
            // ������� ���� Ǯ�� �ְ� �ʹ�.
            enemyPool.Add(enemy);
            // ��Ȱ��ȭ ��������
            enemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ð��� �ѹ��� ���� ����� �ʹ�.
        // 1. �ð��� �귶���ϱ�.
        currentTime += Time.deltaTime;
        // 2. �����ð��� �����ϱ�.
        if (currentTime > createTime)
        {
            // 3. ���� ����� �ʹ�.
            // -> ������Ʈ Ǯ�� �ִ� �༮�߿� ��Ȱ��ȭ �Ǿ� �ִ� �༮�� Ȱ��ȭ ��Ű��
            // ���� Ǯ�� ���� �ִٸ�
            if (enemyPool.Count > 0)
            {
                GameObject enemy = enemyPool[0];
                //Ȱ��ȭ ��Ű��
                enemy.SetActive(true);
                // 4. ��ġ.
                int index = Random.Range(0, spawnpoints.Length);
                enemy.transform.position = spawnpoints[index].position;
                currentTime = 0;
                enemyPool.RemoveAt(0);
            }
        }

    }
}