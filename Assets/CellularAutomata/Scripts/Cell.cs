using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell : MonoBehaviour
{
    //Properties
    public States State { get => state; private set => state = value; }
    public float TimeWhenStartedBurning { get => timeWhenStartedBurning; private set => timeWhenStartedBurning = value; }
    //Fields

    //Materials
    [SerializeField]
    Material fresh;
    [SerializeField]
    Material starting;
    [SerializeField]
    Material burning;
    [SerializeField]
    Material burned;

    States state = States.Fresh;


    float timeWhenStartedBurning;
    public enum States
    {
        Air,
        Fresh,
        Starting,
        Burning,
        Burned
    }

    public void SetState(States state)
    {
        this.state = state;
        if(gameObject.activeInHierarchy==false)
        this.gameObject.SetActive(true);
        switch (state)
            {
            case States.Air:
                this.gameObject.SetActive(false);
                break;
            case States.Fresh:
                this.gameObject.GetComponent<MeshRenderer>().material = fresh;
                break;
            case States.Starting:
                this.gameObject.GetComponent<MeshRenderer>().material = starting;
                break;
            case States.Burning:
                timeWhenStartedBurning = Time.time;
                this.gameObject.GetComponent<MeshRenderer>().material = burning;
                break;
            case States.Burned:
                this.gameObject.GetComponent<MeshRenderer>().material = burned;
                break;
        }
          
    }
    public void DeleteCell()
    {
        DestroyImmediate(gameObject);
    }

}
