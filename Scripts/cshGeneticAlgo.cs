using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshGeneticAlgo : MonoBehaviour
{
    static int populationSize = 100;              // ���뺰 ������ ��
    static double mutationRate = 0.01;            // �������� ���� Ȯ��
    static float thresholdError = 10.0f;
    static float targetError = 1.0f; // ���� ��ǥ ����
    static GameObject MainObj; // ��ġ �� ��� ��ü���� ���� �θ�ü
    int generation = 1; // ù ������� ����
    List<List<Vector2>> population = new();

    cshCalcWalldis cshCalcWalldis;
    GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        MainObj = GameObject.FindGameObjectWithTag("Sample");
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        cshCalcWalldis = GameManager.GetComponent<cshCalcWalldis>();
        Geneinit();
    }

    private void Update()
    {
        Genetic();
    }
    // ���� �˰���

    void Geneinit()
    {
        //���뺰 ��ü �� ��ŭ �ʱ� ��ü ���� ����
        for (int i = 0; i < populationSize; i++)
        {
            List<Vector2> Individual = new();
            for (int j = 0; j < MainObj.transform.childCount; j++)
            {
                // �ʱ� ��ü ��ġ
                float x = Random.Range(-5.0f, 5.0f);
                float z = Random.Range(-7.5f, 8.5f);
                Vector2 info = new Vector2(x, z);
                Individual.Add(info);
            }
            population.Add(Individual);
        }
    }
    void Genetic()
    {
        // ���踦 ���� ��ü���� ��� ���� 
        List<List<Vector2>> matingPool = new();
        // n������ ��� ��ü���� ���鼭
        foreach (List<Vector2> individual in population)
        {
            //���յ�
            int fitness = 0;

            for (int i = 0; i < individual.Count; i++)
            {
                // ��� ��ü�� ������ �Ÿ��� ����ѵ�
                float error = cshCalcWalldis.CalcdisWall(individual[i]);
                Debug.Log(error);
                //�� ��ü�� ��ġ�� ������� �������� ���
                if (error > 1.0f)
                {
                    fitness++;
                }
            }
            Debug.Log("==========");
            Debug.Log("fitness = "+fitness);
            //���յ� ��ŭ ��������� ��ü�� �߰��� (���յ��� ���� ���� �ش� ��ü�� �ߺ��Ǿ� ���� ���� ������ ������ ������ �Ĵ뿡 ������ ���ɼ��� ŭ)
            for (int i = 0; i < fitness; i++)
            {
                matingPool.Add(individual);
            }
        }

        // ��������� ������� �ٽ� ���ο� �����ڷ� ��� ���� ��ü
        List<List<Vector2>> newPopulation = new();
        
        for (int i = 0; i < populationSize; i++)
        {
            //����������� �������� �� �θ�ü ����
            List<Vector2> parentA = matingPool[Random.Range(0, matingPool.Count)];
            List<Vector2> parentB = matingPool[Random.Range(0, matingPool.Count)];

            // A�θ�� B�θ𿡼� ��� ������ ������ ����
            int midpoint = Random.Range(0, parentA.Count);

            //����� �� �θ�ü�� ������� A�θ��� ó������ midpoint����, B�θ��� midpoint���� ������ ������ ����� �ռ�
            List<Vector2> child = new();
            for(int j = 0; j< midpoint; j++)
            {
                child.Add(parentA[j]);
            }

            for (int j = midpoint; j < parentA.Count; j++)
            {
                child.Add(parentB[j]);
            }

            //�������� ����
            List<Vector2> mutation = child;
            for (int j = 0; j < mutation.Count; j++)
            {
                //��������Ȯ��(1%) ����
                if (Random.Range(0.0f,1.0f) < mutationRate)
                {
                    //�������̴� �ش� �κ��� ���ڰ� �������� �ٲ��. �̸� ������� ���ù̴ϸ��� �������� �� �ִ�.
                    float x = Random.Range(-5.0f, 5.0f);
                    float z = Random.Range(-7.5f, 8.5f);
                    mutation[j] = new Vector2(x, z); 
                }
            }
            child = mutation;

            //���ο� ���� ������ �߰�
            newPopulation.Add(child);

        }

        //���� ���븦 ���ο� ����� ���� ��
        population = newPopulation;


        //���뺰 ��� ��ü�� ���
        foreach (List<Vector2> pop in population)
        {
            foreach(Vector2 v in pop)
            {
                Debug.Log($"Generation {generation}: {v}");
            }
            
        }
        
        //���� ����
        generation++;

        // while���� ������ ���븦 ã�Ҵٰ� �˸�
        Debug.Log("Target string found!");
    }
}
