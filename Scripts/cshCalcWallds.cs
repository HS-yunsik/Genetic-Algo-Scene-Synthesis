using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshCalcWalldis : MonoBehaviour
{

    GameObject Wall;
    int Wallnum = 0; // ���� ����� �� index

    private void Awake()
    {
        Wall = GameObject.FindGameObjectWithTag("Wall");
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
