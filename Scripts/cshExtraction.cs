using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshExtraction : MonoBehaviour
{
    public float priorDis;
    public float priorTheta;

    GameObject Wall;
    int Wallnum = 0; // ���� ����� �� index

    // Start is called before the first frame update
    void Start()
    {
        Wall = GameObject.FindGameObjectWithTag("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        PriorData();
    }

    void PriorData()
    {
        float dis = 1000.0f;
        float angle;

        //���� ������ inside�� ��ġ�� �����Ǵ� �� ã��
        for (int i = 0; i < Wall.transform.childCount; i++)
        {
            Vector3 ObjPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
            Vector3 wallPos = new Vector3(Wall.transform.GetChild(i).position.x, 0.0f, Wall.transform.GetChild(i).transform.position.z);
            Vector3 newdis = (ObjPos - wallPos);
            newdis = Vector3.Project(newdis, Wall.transform.GetChild(i).transform.forward);
            if (newdis.magnitude < dis)
            {
                dis = newdis.magnitude;
                Wallnum = i;
            }
        }
        //�� ������ ��ü�� �չ����� right vector�� ���õǾ�����
        angle = Vector3.Angle(transform.right, Wall.transform.GetChild(Wallnum).transform.forward);
        priorDis = dis;
        priorTheta = angle;
    }

}
