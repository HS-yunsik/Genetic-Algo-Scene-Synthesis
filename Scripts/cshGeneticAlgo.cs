using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshGeneticAlgo : MonoBehaviour
{
    static int populationSize = 100;              // ���뺰 ������ ��
    static double mutationRate = 0.01;            // �������� ���� Ȯ��
    static GameObject MainObj; // ��ġ �� ��� ��ü���� ���� �θ�ü

    int generation = 1; // ù ������� ����
    List<List<Vector2>> population = new();
    List<float> priordis = new();


    bool find = false; // ���� �ظ� ã���� ��츦 üũ�ϴ� ����
    cshCalcWalldis cshCalcWalldis;
    GameObject GameManager;
    
    
    public float errorthreshold = 0.5f;
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
        //�����ظ� ã���� ��� update�� ����
        if (find)
            return;
        Genetic();
    }

    // �ʱ�ȭ�ܰ�
    void Geneinit()
    {
        //���뺰 ��ü �� ��ŭ �ʱ� ��ü ���� ����
        for (int i = 0; i < populationSize; i++)
        {
            List<Vector2> Individual = new();
            for (int j = 0; j < MainObj.transform.childCount; j++)
            {
                // �ʱ� ��ü ��ġ
                // 3D Scene�� x, z�� ����������� �������� ��ü ��ġ
                float x = Random.Range(-5.0f, 5.0f);
                float z = Random.Range(-7.5f, 8.5f);
                Vector2 info = new Vector2(x, z);
                Individual.Add(info);   
            }
            population.Add(Individual);
        }

        //���÷κ��� ��ü���� priordis ���� �����ͼ� priordis ����Ʈ�� ����
        for (int i = 0; i < MainObj.transform.childCount; i++)
        {
            cshCurrentData cshCurrentData = MainObj.transform.GetChild(i).GetComponent<cshCurrentData>();
            cshCurrentData.PriorData();
            priordis.Add(cshCurrentData.priorDis);

        }
    }

    //���� ���� �˰���
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
                // ��� ��ü�� ����(���� ��ġ���� ������ �Ÿ��� �̻����� ������ �Ÿ��� ���̸� ���
                float error = Mathf.Abs(cshCalcWalldis.CalcdisWall(individual[i]) - priordis[i]);

                //�� ��ü�� ���̰� ������ �����ϰ�� ���յ� ����
                if (error < errorthreshold)
                {
                    fitness++;
                }
            }
            //���յ� ��ŭ ��������� ��ü�� �߰��� (���յ��� ���� ���� �ش� ��ü�� �ߺ��Ǿ� ���� ���� ������ ������ ������ �Ĵ뿡 ������ ���ɼ��� ŭ)
            for (int i = 0; i < fitness; i++)
            {
                matingPool.Add(individual);
            }

            //��� ��ü�� ���յ��� ������ ���(���� �ظ� ã���� ���)
            if (fitness == MainObj.transform.childCount)
            {
                Debug.Log("find");
                find = true;
                for (int i = 0; i < individual.Count; i++)
                {
                    MainObj.transform.GetChild(i).position = new Vector3(individual[i].x, MainObj.transform.GetChild(i).position.y, individual[i].y);
                } 
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
                if (Random.Range(0.0f, 1.0f) < mutationRate)
                {
                    //�������̴� �ش� �κ��� ���ڰ� �������� �ٲ��. �̸� ������� ���ù̴ϸ��� �������� �� �ִ�.
                    //�������� ��ü ��ġ
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
                //Debug.Log($"Generation {generation}: {v}");
            }
            
        }
        Debug.Log($"Generation {generation}");

        //���� ����
        generation++;
    }
}
