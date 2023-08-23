using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshCurrentData : MonoBehaviour
{
    public float priorDis;
    public float priorTheta;
    public float curDis;
    public float curTheta;
    public float curError;

    float wd = 1f;
    float wr = 0.1f;

    cshCalcWalldis cshCalcWalldis;
    GameObject Wall;
    int Wallnum = 0; // ���� ����� �� index

    private void Awake()
    {
        Wall = GameObject.FindGameObjectWithTag("Wall");
    }


    // Start is called before the first frame update
    void Start()
    {
        //���÷κ��� ������ ����
        PriorData();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentData();
        CurrentError();
    }
    void CurrentError()
    {
        curError = Mathf.Abs(priorDis - curDis) * wd + Mathf.Abs(priorTheta - curTheta) * wr;
    }
    void CurrentData()
    {
        //float angle;
        Vector2 xz = new Vector2(transform.position.x, transform.position.z);
        curDis = CalcdisWall(xz);
        //�� ������ ��ü�� �չ����� right vector�� ���õǾ�����
        //angle = Vector3.Angle(transform.right, Wall.transform.GetChild(Wallnum).transform.forward);
    }

    //���÷κ��� ������ ����
    public void PriorData()
    {
        Vector2 xz = new Vector2(transform.position.x, transform.position.z);
        priorDis = CalcdisWall(xz);
        //�� ������ ��ü�� �չ����� right vector�� ���õǾ�����
        //angle = Vector3.Angle(transform.right, Wall.transform.GetChild(Wallnum).transform.forward);
    }

    //xz ��ġ ��ǥ�� �޾Ƽ� ���� ����� ������ �Ÿ��� ��ȯ���ִ� �Լ�
    public float CalcdisWall(Vector2 xz)
    {
        float dis = 1000.0f;
        //float angle;

        //���� ����� ��ã�Ƽ� �Ÿ� ��ȯ
        for (int i = 0; i < Wall.transform.childCount; i++)
        {
            Vector3 ObjPos = new Vector3(xz.x, 0.0f, xz.y);
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
        //angle = Vector3.Angle(transform.right, Wall.transform.GetChild(Wallnum).transform.forward);
        //priorTheta = angle;

        return dis;
    }
}
