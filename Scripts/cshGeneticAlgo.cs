using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshGeneticAlgo : MonoBehaviour
{
    static int populationSize = 100;              // 세대별 유전자 수
    static double mutationRate = 0.01;            // 돌연변이 생성 확률
    static GameObject MainObj; // 배치 할 모든 객체들을 담은 부모객체

    int generation = 1; // 첫 세대부터 시작
    List<List<List<float>>> population = new();
    List<float> priordis = new();
    List<float> priorrot = new();


    bool find = false; // 최적 해를 찾았을 경우를 체크하는 변수
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
        //최적해를 찾았을 경우 update문 종료
        if (find)
            return;
        Genetic();
    }

    // 초기화단계
    void Geneinit()
    {
        //세대별 객체 수 만큼 초기 객체 정보 설정
        for (int i = 0; i < populationSize; i++)
        {
            List<List<float>> Individual = new();
            for (int j = 0; j < MainObj.transform.childCount; j++)
            {
                // 초기 객체 위치
                // 3D Scene의 x, z축 가용범위에서 무작위로 객체 배치
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

        //샘플로부터 객체들의 priordis 정보 가져와서 priordis 리스트에 저장
        for (int i = 0; i < MainObj.transform.childCount; i++)
        {
            cshCurrentData cshCurrentData = MainObj.transform.GetChild(i).GetComponent<cshCurrentData>();
            cshCurrentData.PriorData();
            priordis.Add(cshCurrentData.priorDis);
            priorrot.Add(cshCurrentData.priorTheta);
        }
    }

    //메인 유전 알고리즘 (한번 돌면 한세대 진행)
    void Genetic()
    {
        // 교배를 위한 객체들을 담는 공간 
        List<List<List<float>>> matingPool = new();

        // 해당 세대에서 가장 우월한 자식의 적합도
        int bestFitness = 0;
        List<List<float>> bestIndividual = new();

        // n세대의 모든 객체들을 돌면서
        foreach (List<List<float>> individual in population)
        {
            //적합도
            int fitness = 0;

            for (int i = 0; i < individual.Count; i++)
            {
                // 모든 객체의 에러(현재 위치에서 벽과의 거리와 이상적인 벽과의 거리의 차이를 계산
                // individual[i][0], individual[i][1], individual[i][2] --> x좌표, y좌표, 벽과의 theta
                Vector2 disrot = cshCalcWalldis.CalcpriorWall(individual[i][0], individual[i][1], individual[i][2]);
                float diserror = Mathf.Abs(disrot.x - priordis[i]);
                float roterror = Mathf.Abs(disrot.y - priorrot[i]);

                float totalerror = diserror + roterror;
                //각 객체의 차이가 일정값 이하일경우 적합도 증가
                if (totalerror < errorthreshold)
                {
                    fitness++;
                }
            }
            //적합도 만큼 교배공간에 객체를 추가함 (적합도가 높을 수록 해당 객체가 중복되어 많이 들어가기 때문에 본인의 정보를 후대에 전달할 가능성이 큼)
            for (int i = 0; i < fitness; i++)
            {
                matingPool.Add(individual);
            }
            
            if(fitness > bestFitness)
            {
                bestFitness = fitness;
                bestIndividual = individual;
            }

            //모든 객체가 적합도를 만족할 경우(최적 해를 찾았을 경우)
            if (fitness == MainObj.transform.childCount)
            {
                Debug.Log("find");
                find = true;
                for (int i = 0; i < individual.Count; i++)
                {
                    //individual[i][0] --> x좌표
                    //individual[i][1] --> z좌표
                    MainObj.transform.GetChild(i).position = new Vector3(individual[i][0], MainObj.transform.GetChild(i).position.y, individual[i][1]);
                    MainObj.transform.GetChild(i).eulerAngles = new Vector3(0, individual[i][2], 0);
                } 
            }
        }

        //현재 세대에서 가장 우월한 정보로 객체 배치
        for (int i = 0; i < bestIndividual.Count; i++)
        {
            //bestIndividual[i][0] --> x좌표
            //bestIndividual[i][1] --> z좌표
            //bestIndividual[i][2] --> y축 회전값
            MainObj.transform.GetChild(i).position = new Vector3(bestIndividual[i][0], MainObj.transform.GetChild(i).position.y, bestIndividual[i][1]);
            MainObj.transform.GetChild(i).eulerAngles = new Vector3(0, bestIndividual[i][2], 0);
        }

        // 교배공간을 기반으로 다시 새로운 유전자로 모든 세대 교체
        List<List<List<float>>> newPopulation = new();
        
        for (int i = 0; i < populationSize; i++)
        {
            //교배공간에서 랜덤으로 두 부모객체 추출
            List<List<float>> parentA = matingPool[Random.Range(0, matingPool.Count)];
            List<List<float>> parentB = matingPool[Random.Range(0, matingPool.Count)];

            // A부모와 B부모에서 섞어서 가져올 기준점 생성
            int midpoint = Random.Range(0, parentA.Count);

            //추출된 두 부모객체를 기반으로 A부모의 처음부터 midpoint까지, B부모의 midpoint부터 끝까지 추출한 결과를 합성
            List<List<float>> child = new();
            for(int j = 0; j< midpoint; j++)
            {
                child.Add(parentA[j]);
            }

            for (int j = midpoint; j < parentA.Count; j++)
            {
                child.Add(parentB[j]);
            }

            //돌연변이 생성
            List<List<float>> mutation = child;
            for (int j = 0; j < mutation.Count; j++)
            {
                //돌연변이확률(1%) 적용
                if (Random.Range(0.0f, 1.0f) < mutationRate)
                {
                    //돌연변이는 해당 부분의 글자가 랜덤으로 바뀐다. 이를 기반으로 로컬미니마를 빠져나갈 수 있다.
                    //무작위로 객체 배치
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
                    // todo 회전돌연변이
                }
            }
            child = mutation;

            //새로운 세대 공간에 추가
            newPopulation.Add(child);

        }

        //기존 세대를 새로운 세대로 덮어 씀
        population = newPopulation;


        //세대별 모든 객체들 출력
        foreach (List<List<float>> pop in population)
        {
            foreach(List<float> v in pop)
            {
                //Debug.Log($"Generation {generation}: {v}");
            }
            
        }
        Debug.Log($"Generation {generation}");

        //세대 증가
        generation++;
    }
}
