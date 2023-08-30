using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshGeneticAlgo : MonoBehaviour
{
    static int populationSize = 100;              // ���뺰 ������ ��
    static double mutationRate = 0.01;            // �������� ���� Ȯ��
    static GameObject MainObj; // ��ġ �� ��� ��ü���� ���� �θ�ü

    int generation = 1; // ù ������� ����
    List<List<List<float>>> population = new();
    List<float> priordis = new();
    List<float> priorrot = new();


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
            List<List<float>> Individual = new();
            for (int j = 0; j < MainObj.transform.childCount; j++)
            {
                // �ʱ� ��ü ��ġ
                // 3D Scene�� x, z�� ����������� �������� ��ü ��ġ
                float x = Random.Range(-5.0f, 5.0f);
                float z = Random.Range(-7.5f, 8.5f);
                float theta = Random.Range(-2.0f, 2.0f);
                List<float> info = new();
                info.Add(x);
                info.Add(z);
                info.Add(theta*90);
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
            priorrot.Add(cshCurrentData.priorTheta);
        }
    }

    //���� ���� �˰��� (�ѹ� ���� �Ѽ��� ����)
    void Genetic()
    {
        // ���踦 ���� ��ü���� ��� ���� 
        List<List<List<float>>> matingPool = new();

        // �ش� ���뿡�� ���� ����� �ڽ��� ���յ�
        int bestFitness = 0;
        List<List<float>> bestIndividual = new();

        // n������ ��� ��ü���� ���鼭
        foreach (List<List<float>> individual in population)
        {
            //���յ�
            int fitness = 0;

            for (int i = 0; i < individual.Count; i++)
            {
                // ��� ��ü�� ����(���� ��ġ���� ������ �Ÿ��� �̻����� ������ �Ÿ��� ���̸� ���
                // individual[i][0], individual[i][1], individual[i][2] --> x��ǥ, y��ǥ, ������ theta
                Vector2 disrot = cshCalcWalldis.CalcpriorWall(individual[i][0], individual[i][1], individual[i][2]);
                float diserror = Mathf.Abs(disrot.x - priordis[i]);
                float roterror = Mathf.Abs(disrot.y - priorrot[i]);

                float totalerror = diserror + roterror;
                //�� ��ü�� ���̰� ������ �����ϰ�� ���յ� ����
                if (totalerror < errorthreshold)
                {
                    fitness++;
                }
            }
            //���յ� ��ŭ ��������� ��ü�� �߰��� (���յ��� ���� ���� �ش� ��ü�� �ߺ��Ǿ� ���� ���� ������ ������ ������ �Ĵ뿡 ������ ���ɼ��� ŭ)
            for (int i = 0; i < fitness; i++)
            {
                matingPool.Add(individual);
            }
            
            if(fitness > bestFitness)
            {
                bestFitness = fitness;
                bestIndividual = individual;
            }

            //��� ��ü�� ���յ��� ������ ���(���� �ظ� ã���� ���)
            if (fitness == MainObj.transform.childCount)
            {
                Debug.Log("find");
                find = true;
                for (int i = 0; i < individual.Count; i++)
                {
                    //individual[i][0] --> x��ǥ
                    //individual[i][1] --> z��ǥ
                    MainObj.transform.GetChild(i).position = new Vector3(individual[i][0], MainObj.transform.GetChild(i).position.y, individual[i][1]);
                    MainObj.transform.GetChild(i).eulerAngles = new Vector3(0, individual[i][2], 0);
                } 
            }
        }

        //���� ���뿡�� ���� ����� ������ ��ü ��ġ
        for (int i = 0; i < bestIndividual.Count; i++)
        {
            //bestIndividual[i][0] --> x��ǥ
            //bestIndividual[i][1] --> z��ǥ
            //bestIndividual[i][2] --> y�� ȸ����
            MainObj.transform.GetChild(i).position = new Vector3(bestIndividual[i][0], MainObj.transform.GetChild(i).position.y, bestIndividual[i][1]);
            MainObj.transform.GetChild(i).eulerAngles = new Vector3(0, bestIndividual[i][2], 0);
        }

        // ��������� ������� �ٽ� ���ο� �����ڷ� ��� ���� ��ü
        List<List<List<float>>> newPopulation = new();
        
        for (int i = 0; i < populationSize; i++)
        {
            //����������� �������� �� �θ�ü ����
            List<List<float>> parentA = matingPool[Random.Range(0, matingPool.Count)];
            List<List<float>> parentB = matingPool[Random.Range(0, matingPool.Count)];

            // A�θ�� B�θ𿡼� ��� ������ ������ ����
            int midpoint = Random.Range(0, parentA.Count);

            //����� �� �θ�ü�� ������� A�θ��� ó������ midpoint����, B�θ��� midpoint���� ������ ������ ����� �ռ�
            List<List<float>> child = new();
            for(int j = 0; j< midpoint; j++)
            {
                child.Add(parentA[j]);
            }

            for (int j = midpoint; j < parentA.Count; j++)
            {
                child.Add(parentB[j]);
            }

            //�������� ����
            List<List<float>> mutation = child;
            for (int j = 0; j < mutation.Count; j++)
            {
                //��������Ȯ��(1%) ����
                if (Random.Range(0.0f, 1.0f) < mutationRate)
                {
                    //�������̴� �ش� �κ��� ���ڰ� �������� �ٲ��. �̸� ������� ���ù̴ϸ��� �������� �� �ִ�.
                    //�������� ��ü ��ġ
                    float x = Random.Range(-5.0f, 5.0f);
                    float z = Random.Range(-7.5f, 8.5f);
                    float theta = Random.Range(-2, 2);
                    mutation[j] = new();
                    mutation[j].Add(x);
                    mutation[j].Add(z);
                    mutation[j].Add(theta*90);
                    //mutation[j] = new Vector2(x, z);
                }
                if (Random.Range(0.0f, 1.0f) < mutationRate)
                {
                    // todo ȸ����������
                }
            }
            child = mutation;

            //���ο� ���� ������ �߰�
            newPopulation.Add(child);

        }

        //���� ���븦 ���ο� ����� ���� ��
        population = newPopulation;


        //���뺰 ��� ��ü�� ���
        foreach (List<List<float>> pop in population)
        {
            foreach(List<float> v in pop)
            {
                //Debug.Log($"Generation {generation}: {v}");
            }
            
        }
        Debug.Log($"Generation {generation}");

        //���� ����
        generation++;
    }
}
