using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanController : MonoBehaviour
{
    [SerializeField]
    private GameObject greenBean;

    [SerializeField]
    private GameObject goldBean;

    [SerializeField]
    private Vector2[] beanList = new Vector2[3];

    Queue<GameObject> BeanQ = new Queue<GameObject>();
    private int Count = 0;

    public void MakeBean(float i)
    {
        for (int j = 0; j < 3; j++)
            beanList[j].y = i;

        if(BeanQ.Count > 8)
            BeanQ.Dequeue();

        if(Random.Range(0,5) > 2)
            SelectLocation(SelectBean(),1,i);
        else if (Random.Range(0,5) > 1)
            SelectLocation(SelectBean(),2,i);
        
    }

    private GameObject SelectBean()
    {
        GameObject Bean;
        if (Random.Range(0, 100) > 5)
            Bean = Instantiate(greenBean);
        else
            Bean = Instantiate(goldBean);
        BeanQ.Enqueue(Bean);
        return Bean;
    }

    private void SelectLocation(GameObject Bean, int i,float y)
    {
        int ran = Random.Range(0,3);
        Bean.transform.position = beanList[ran];
        Bean.transform.parent = FindObjectOfType<FloorController>().transform;

        GameObject newBean;
        if(i== 2)
        {
            int ran2 = Random.Range(0, 3);
            while (ran2 == ran)
                ran2 = Random.Range(0, 3);
            newBean = SelectBean();
            newBean.transform.position = beanList[ran2];
            newBean.transform.parent = FindObjectOfType<FloorController>().transform;
        }     
    }
}
