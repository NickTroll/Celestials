using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
  
{
    private GameObject player;
    private GameObject cam;
    public bool finished = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (finished == true)
        {
            cam = GameObject.FindWithTag("Start");
            transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10);

        }
        else
        {
            player = GameObject.FindWithTag("Player");
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        }
    }

    public void SetTrue()
    {
        finished = true;
    }
}
