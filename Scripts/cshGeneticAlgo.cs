using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshGeneticAlgo : MonoBehaviour
{
    static int populationSize = 100;              // 세대별 유전자 수
    static double mutationRate = 0.01;            // 돌연변이 생성 확률
    static float thresholdError = 10.0f;
    static float targetError = 1.0f; // 최종 목표 에러
    static GameObject MainObj; // 배치 할 모든 객체들을 담은 부모객체
    int generation = 1; // 첫 세대부터 시작
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
    // 유전 알고리즘

    void Geneinit()
    {
        //세대별 객체 수 만큼 초기 객체 정보 설정
        for (int i = 0; i < populationSize; i++)
        {
            List<Vector2> Individual = new();
            for (int j = 0; j < MainObj.transform.childCount; j++)
            {
                // 초기 객체 위치
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
        // 교배를 위한 객체들을 담는 공간 
        List<List<Vector2>> matingPool = new();
        // n세대의 모든 객체들을 돌면서
        foreach (List<Vector2> individual in population)
        {
            //적합도
            int fitness = 0;

            for (int i = 0; i < individual.Count; i++)
            {
                // 모든 객체의 벽과의 거리를 계산한뒤
                float error = cshCalcWalldis.CalcdisWall(individual[i]);
                Debug.Log(error);
                //각 객체의 위치를 기반으로 에러값을 계산
                if (error > 1.0f)
                {
                    fitness++;
                }
            }
            Debug.Log("==========");
            Debug.Log("fitness = "+fitness);
            //적합도 만큼 교배공간에 객체를 추가함 (적합도가 높을 수록 해당 객체가 중복되어 많이 들어가기 때문에 본인의 정보를 후대에 전달할 가능성이 큼)
            for (int i = 0; i < fitness; i++)
            {
                matingPool.Add(individual);
            }
        }

        // 교배공간을 기반으로 다시 새로운 유전자로 모든 세대 교체
        List<List<Vector2>> newPopulation = new();
        
        for (int i = 0; i < populationSize; i++)
        {
            //교배공간에서 랜덤으로 두 부모객체 추출
            List<Vector2> parentA = matingPool[Random.Range(0, matingPool.Count)];
            List<Vector2> parentB = matingPool[Random.Range(0, matingPool.Count)];

            // A부모와 B부모에서 섞어서 가져올 기준점 생성
            int midpoint = Random.Range(0, parentA.Count);

            //추출된 두 부모객체를 기반으로 A부모의 처음부터 midpoint까지, B부모의 midpoint부터 끝까지 추출한 결과를 합성
            List<Vector2> child = new();
            for(int j = 0; j< midpoint; j++)
            {
                child.Add(parentA[j]);
            }

            for (int j = midpoint; j < parentA.Count; j++)
            {
                child.Add(parentB[j]);
            }

            //돌연변이 생성
            List<Vector2> mutation = child;
            for (int j = 0; j < mutation.Count; j++)
            {
                //돌연변이확률(1%) 적용
                if (Random.Range(0.0f,1.0f) < mutationRate)
                {
                    //돌연변이는 해당 부분의 글자가 랜덤으로 바뀐다. 이를 기반으로 로컬미니마를 빠져나갈 수 있다.
                    float x = Random.Range(-5.0f, 5.0f);
                    float z = Random.Range(-7.5f, 8.5f);
                    mutation[j] = new Vector2(x, z); 
                }
            }
            child = mutation;

            //새로운 세대 공간에 추가
            newPopulation.Add(child);

        }

        //기존 세대를 새로운 세대로 덮어 씀
        population = newPopulation;


        //세대별 모든 객체들 출력
        foreach (List<Vector2> pop in population)
        {
            foreach(Vector2 v in pop)
            {
                Debug.Log($"Generation {generation}: {v}");
            }
            
        }
        
        //세대 증가
        generation++;

        // while문이 끝나면 세대를 찾았다고 알림
        Debug.Log("Target string found!");
    }
}
