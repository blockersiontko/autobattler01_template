using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ArenaManager : MonoBehaviour
{
    public knight Fighter1;
    public archer Fighter2;

    private void Start()
    {
        StartCoroutine(StartDuel());
    }

    private IEnumerator StartDuel()
    {
        Debug.Log("Walka siê zaczyna.");
        Debug.Log("-------------------");
        while (Fighter1.isAlive && Fighter2.isAlive)
        {
            Fighter1.Attack(Fighter2);
            Debug.Log($"{Fighter1} ma {Fighter1._CurrentHP} HP.");
            Debug.Log($"{Fighter2} ma {Fighter2._CurrentHP} HP.");
            Debug.Log("-------------------");
            yield return new WaitForSeconds(2f);

            if (!Fighter2.isAlive) break;

            Fighter2.Attack(Fighter1);
            Debug.Log($"{Fighter1} ma {Fighter1._CurrentHP} HP.");
            Debug.Log($"{Fighter2} ma {Fighter2._CurrentHP} HP.");
            Debug.Log("-------------------");
            yield return new WaitForSeconds(2f);
        }

        if (!Fighter1.isAlive)
        {
            Debug.Log($"{Fighter2} zwyciê¿a!");
        }

        if (!Fighter2.isAlive)
        {
            Debug.Log($"{Fighter1} zwyciê¿a!");
        }
    }
}