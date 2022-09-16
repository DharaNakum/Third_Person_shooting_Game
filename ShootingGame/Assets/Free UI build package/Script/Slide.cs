using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slide : MonoBehaviour {
	Image Filler;
	public Slider slider;

	void Start () {
		Filler = GetComponent<Image>();
	}
	
	void Update () {
		Filler.fillAmount = slider.value;
	}
}
