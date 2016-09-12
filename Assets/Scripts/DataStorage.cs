using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DataStorage : MonoBehaviour
{

    string storage;
    int highestPoint;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        storage = "";
               
    }

    public bool IsPeaked(string toAdd)
    {
        int temp;
        //turn the string into an int
        if (int.TryParse(toAdd, out temp))
        {
            //keep the biggest value, highestPpoint or temp
            if (highestPoint < temp)
            {
                highestPoint = temp;
            }
            //if temp was wayyy smaller than highest point, return true
            else if (temp < highestPoint / 4)
            {
                return true;

            }
        }
        return false;
    }

    public void Add(string toAdd)
    {
        storage = storage + "\n" + toAdd;
        Debug.Log(storage);
    }
    public string GetStorage()
    {
        return storage;
    }
}
