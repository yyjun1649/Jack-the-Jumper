using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private Transform Monster;

    [SerializeField]
    private bool isRight = true;

    private Vector3 finishPivot;

    bool flag = false;
    bool flag2 = true;
    void Start()
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(StartMoveCoroutine());
    }

    IEnumerator StartMoveCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Monster.GetComponent<Animator>().SetBool("Walk", false);
        yield return new WaitForSeconds(0.5f);
        Monster.GetComponent<Animator>().SetTrigger("Scream");
        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(GoToFinishPivot());
        }
    }

    IEnumerator GoToFinishPivot()
    {
        if (flag2)
        {
            flag2 = false;
            Monster.GetComponent<Animator>().SetTrigger("Scream");
            yield return new WaitForSeconds(1f);
            Monster.GetComponent<Animator>().SetBool("Run",true);
            transform.GetComponent<Animator>().SetBool("Run", true);
        }
    }
}
