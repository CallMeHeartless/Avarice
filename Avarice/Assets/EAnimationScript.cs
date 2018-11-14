using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EAnimationScript : MonoBehaviour {

    public EnemyAI Enemy;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Lunge()
    {
        Enemy.Lunge(0.6f);
    }

    void ResetSpeed()
    {

    }
}
