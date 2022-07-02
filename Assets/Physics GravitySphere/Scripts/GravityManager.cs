using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityManager : MonoBehaviour
{
    [Range(0f, 100)]
    [SerializeField]
    int randomGravityReceiverAmount;
    [SerializeField]
    GameObject gravityReceiverPrefab;
    [SerializeField]
    GameObject gravityTriggerPrefab;
    [SerializeField]
    float mass;
    GameObject currentGravityTrigger;
    List<GameObject> currentGravityReceivers = new List<GameObject>();
    public void LoadGravityShowcaseScene()
    {
        if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
            return;
        SceneManager.LoadScene(1, LoadSceneMode.Additive);      
    }
    public void FindGravityObjectsInSceneWithDelay(float delay)
    {
        Invoke("FindGravityObjectsInScene", delay);
    }
    public void FindGravityObjectsInScene()
    {
        currentGravityTrigger = GameObject.Find("GravityTrigger");
        foreach (GameObject gravityReceiver in GameObject.FindGameObjectsWithTag("gravityAffected"))
        {
            currentGravityReceivers.Add(gravityReceiver);
        }
    }
    public void UnloadGravityShowcaseScene()
    {
        SceneManager.UnloadScene(1);
        DestroyGravityTrigger();
        DestroyCurrentGravityReceivers();

    }
    public void CreateGravityReceivers()
    {
        for (int x = -10; x < 11; x++)
        {
            for (int z = -10; z < 11; z++)
            {
                int random = (int)Random.Range(0, 101);
                if (random < randomGravityReceiverAmount)
                {
                    GameObject gravityReceiver = Instantiate(gravityReceiverPrefab);
                    gravityReceiver.transform.position = new Vector3(x, 1, z);
                    currentGravityReceivers.Add(gravityReceiver);
                }

            }
        }
    }
    public void DestroyCurrentGravityReceivers()
    {
        foreach (GameObject gravityReceiver in currentGravityReceivers)
        {
            Destroy(gravityReceiver);
        }
        currentGravityReceivers.Clear();
    }
    public void ActivateGravity()
    {
        foreach (GameObject gravityReceiver in currentGravityReceivers)
        {
            gravityReceiver.GetComponent<GravityReceiver>().GravityActive = true;
        }
    }
    public void DeactivateGravity()
    {
        foreach (GameObject gravityReceiver in currentGravityReceivers)
        {
            gravityReceiver.GetComponent<GravityReceiver>().GravityActive = false;
        }
    }
    public void CreateGravityTrigger()
    {
        if(currentGravityTrigger != null)
            Destroy(currentGravityTrigger);

       currentGravityTrigger = Instantiate(gravityTriggerPrefab);
        currentGravityTrigger.transform.position = new Vector3(0, 10, 0);
        currentGravityTrigger.GetComponent<GravityTrigger>().Mass = mass;
    }
    public void DestroyGravityTrigger()
    {
        Destroy(currentGravityTrigger);
    }
}
