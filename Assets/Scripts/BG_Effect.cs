using UnityEngine;
using UnityEngine.UI;

public class BG_Effect : MonoBehaviour {

	public Image sanityBar;
    InsanityManager sanityScript;
    Animator anim;
	
    void Start () {
        anim = GetComponent<Animator>();
        sanityScript = FindObjectOfType<InsanityManager>();
	}
	
	void Update () {
        anim.SetFloat("currentSanity", sanityScript.currentSanity);
	}
}
