using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo : MonoBehaviour
{
    static System.Random random = new();          // ������ ������ ���� random ��ü
    static string targetString = "Hello, world!"; // ���� ��ǥ ���ڿ�
    static int populationSize = 100;              // ���뺰 ������ ��
    static double mutationRate = 0.01;            // �������� ���� Ȯ��

 
    // Start is called before the first frame update
    void Start()
    {
        Genetic();
    }
    
    // ���� �˰���
    static void Genetic()
    {
        //�ʱ�ȭ �ܰ�
        List<string> population = new(); //

        //���뺰 ��ü �� ��ŭ �ʱ� ��ü ���� ����
        for (int i = 0; i < populationSize; i++)
        {
            // �ʱ� ��ü ���� ����
            float[] priordis = new float[]

            // ������ ���ڿ� ����
            char[] chars = new char[targetString.Length];
            for (int j = 0; j < targetString.Length; j++)
            {
                chars[j] = (char)random.Next(32, 127); // ASCII range
            }
            string individual = new(chars);
            
            //���뺰 �ʱ� ��ü �߰�
            population.Add(individual);
        }

        int generation = 1; // ù ������� ����

        //��� ���밡 targetString("hello, world")�� ����� �� �� ���� ����
        while (!population.Contains(targetString))
        {
            // ���踦 ���� ��ü���� ��� ���� 
            List<string> matingPool = new();

            // n������ ��� ��ü���� ���鼭
            foreach (string individual in population)
            {
                //���յ�
                int fitness = 0;

                for (int i = 0; i < individual.Length; i++)
                {
                    //�� ��ü�� ö�� �ϳ��� ���ϸ鼭 ���յ� ����
                    if (individual[i] == targetString[i])
                    {
                        fitness++;
                    }
                }

                //���յ� ��ŭ ��������� ��ü�� �߰��� (���յ��� ���� ���� �ش� ��ü�� �ߺ��Ǿ� ���� ���� ������ ������ ������ �Ĵ뿡 ������ ���ɼ��� ŭ)
                for (int i = 0; i < fitness; i++)
                {
                    matingPool.Add(individual);
                }
            }

            // ��������� ������� �ٽ� ���ο� �����ڷ� ��� ���� ��ü
            List<string> newPopulation = new();

            for (int i = 0; i < populationSize; i++)
            {
                //����������� �������� �� �θ�ü ����
                string parentA = matingPool[random.Next(matingPool.Count)];
                string parentB = matingPool[random.Next(matingPool.Count)];

                // A�θ�� B�θ𿡼� ��� ������ ������ ����
                int midpoint = random.Next(parentA.Length);

                //����� �� �θ�ü�� ������� A�θ��� ù�ܾ���� midpoint����, B�θ��� midpoint���� ������ ������ ����� �ռ�
                string child = parentA.Substring(0, midpoint) + parentB.Substring(midpoint);

                //�������� ����
                char[] chars = child.ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    //��������Ȯ��(1%) ����
                    if (random.NextDouble() < mutationRate)
                    {
                        //�������̴� �ش� �κ��� ���ڰ� �������� �ٲ��. �̸� ������� ���ù̴ϸ��� �������� �� �ִ�.
                        chars[j] = (char)random.Next(32, 127);
                    }
                }
                child = new string(chars);

                //���ο� ���� ������ �߰�
                newPopulation.Add(child);
            }

            //���� ���븦 ���ο� ����� ���� ��
            population = newPopulation;


            //���뺰 ��� ��ü�� ���
            foreach (string pop in population)
            {
                Debug.Log($"Generation {generation}: {pop}");
            }
            
            //���� ����
            generation++;
        }
        // while���� ������ ���븦 ã�Ҵٰ� �˸�
        Debug.Log("Target string found!");
    }
    */
}
