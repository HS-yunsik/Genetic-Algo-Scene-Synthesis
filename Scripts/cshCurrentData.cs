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

    GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        cshCalcWalldis = GameManager.GetComponent<cshCalcWalldis>();
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
        curDis = cshCalcWalldis.CalcdisWall(xz);
        //�� ������ ��ü�� �չ����� right vector�� ���õǾ�����
        //angle = Vector3.Angle(transform.right, Wall.transform.GetChild(Wallnum).transform.forward);
    }

    //���÷κ��� ������ ����
    void PriorData()
    {
        Vector2 xz = new Vector2(transform.position.x, transform.position.z);
        priorDis = cshCalcWalldis.CalcdisWall(xz);
        //�� ������ ��ü�� �չ����� right vector�� ���õǾ�����
        //angle = Vector3.Angle(transform.right, Wall.transform.GetChild(Wallnum).transform.forward);
    }
}
