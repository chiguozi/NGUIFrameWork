using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIManager.Init();
        UIManager.Instance.ShowPanel<TextPanel>();
        UIManager.Instance.ShowPanel<TextPanel1>();
        UIManager.Instance.ShowPanel<TextPanel>();
        //UIManager.Instance.HidePanel("TextPanel");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
