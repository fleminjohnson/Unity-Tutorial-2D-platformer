using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myscript : MonoBehaviour
{
    //Comments
    //Variables
    //control flow
    //loops
    int playerscore = 11;
    float playerspeed = 5.5f;
    string playerName = "Alice";
    bool isGameOver = false;

    //operators

    // Start is called before the first frame undate
    void Start()
    {
        int j = 0;

        j += 1;

        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Hello world");
        }
    }
}
