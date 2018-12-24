using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

    public static GameObject Instance;

    void Awake () {
        if (Instance) {
            Destroy (gameObject);
        } else {
            Instance = gameObject;
        }
    }

	void Start () {
		//Causes UI object not to be destroyed when loading a new scene. If you want it to be destroyed, destroy it manually via script.
		DontDestroyOnLoad(gameObject);
	}

	

}
