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
        //샘플로부터 데이터 추출
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
        //본 씬에서 객체의 앞방향이 right vector로 세팅되어있음
        //angle = Vector3.Angle(transform.right, Wall.transform.GetChild(Wallnum).transform.forward);
    }

    //샘플로부터 데이터 추출
    void PriorData()
    {
        Vector2 xz = new Vector2(transform.position.x, transform.position.z);
        priorDis = cshCalcWalldis.CalcdisWall(xz);
        //본 씬에서 객체의 앞방향이 right vector로 세팅되어있음
        //angle = Vector3.Angle(transform.right, Wall.transform.GetChild(Wallnum).transform.forward);
    }
}
