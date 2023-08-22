using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo : MonoBehaviour
{
    static System.Random random = new();          // 랜덤값 추출을 위한 random 객체
    static string targetString = "Hello, world!"; // 최종 목표 문자열
    static int populationSize = 100;              // 세대별 유전자 수
    static double mutationRate = 0.01;            // 돌연변이 생성 확률

 
    // Start is called before the first frame update
    void Start()
    {
        Genetic();
    }
    
    // 유전 알고리즘
    static void Genetic()
    {
        //초기화 단계
        List<string> population = new(); //

        //세대별 객체 수 만큼 초기 객체 정보 설정
        for (int i = 0; i < populationSize; i++)
        {
            // 초기 객체 정보 추출
            float[] priordis = new float[]

            // 랜덤한 문자열 생성
            char[] chars = new char[targetString.Length];
            for (int j = 0; j < targetString.Length; j++)
            {
                chars[j] = (char)random.Next(32, 127); // ASCII range
            }
            string individual = new(chars);
            
            //세대별 초기 객체 추가
            population.Add(individual);
        }

        int generation = 1; // 첫 세대부터 시작

        //어느 세대가 targetString("hello, world")를 만들어 낼 때 까지 실행
        while (!population.Contains(targetString))
        {
            // 교배를 위한 객체들을 담는 공간 
            List<string> matingPool = new();

            // n세대의 모든 객체들을 돌면서
            foreach (string individual in population)
            {
                //적합도
                int fitness = 0;

                for (int i = 0; i < individual.Length; i++)
                {
                    //각 객체의 철자 하나씩 비교하면서 적합도 판정
                    if (individual[i] == targetString[i])
                    {
                        fitness++;
                    }
                }

                //적합도 만큼 교배공간에 객체를 추가함 (적합도가 높을 수록 해당 객체가 중복되어 많이 들어가기 때문에 본인의 정보를 후대에 전달할 가능성이 큼)
                for (int i = 0; i < fitness; i++)
                {
                    matingPool.Add(individual);
                }
            }

            // 교배공간을 기반으로 다시 새로운 유전자로 모든 세대 교체
            List<string> newPopulation = new();

            for (int i = 0; i < populationSize; i++)
            {
                //교배공간에서 랜덤으로 두 부모객체 추출
                string parentA = matingPool[random.Next(matingPool.Count)];
                string parentB = matingPool[random.Next(matingPool.Count)];

                // A부모와 B부모에서 섞어서 가져올 기준점 생성
                int midpoint = random.Next(parentA.Length);

                //추출된 두 부모객체를 기반으로 A부모의 첫단어부터 midpoint까지, B부모의 midpoint부터 끝까지 추출한 결과를 합성
                string child = parentA.Substring(0, midpoint) + parentB.Substring(midpoint);

                //돌연변이 생성
                char[] chars = child.ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    //돌연변이확률(1%) 적용
                    if (random.NextDouble() < mutationRate)
                    {
                        //돌연변이는 해당 부분의 글자가 랜덤으로 바뀐다. 이를 기반으로 로컬미니마를 빠져나갈 수 있다.
                        chars[j] = (char)random.Next(32, 127);
                    }
                }
                child = new string(chars);

                //새로운 세대 공간에 추가
                newPopulation.Add(child);
            }

            //기존 세대를 새로운 세대로 덮어 씀
            population = newPopulation;


            //세대별 모든 객체들 출력
            foreach (string pop in population)
            {
                Debug.Log($"Generation {generation}: {pop}");
            }
            
            //세대 증가
            generation++;
        }
        // while문이 끝나면 세대를 찾았다고 알림
        Debug.Log("Target string found!");
    }
    */
}
