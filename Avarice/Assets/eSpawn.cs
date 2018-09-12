using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eSpawn : MonoBehaviour {

    public int group;
    public string groupName;

    private void setGroupName()
    {
        if (group == 0)
        {
            group = 1;
        }

        if (group == 1)
        {
            groupName = "Group1";
        }

        if (group == 2)
        {
            groupName = "Group2";
        }

        if (group == 3)
        {
            groupName = "Group3";
        }

        if (group == 4)
        {
            groupName = "Group4";
        }

        if (group == 5)
        {
            groupName = "Group5";
        }
    }

	// Use this for initialization
	void Start () {

        setGroupName();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
