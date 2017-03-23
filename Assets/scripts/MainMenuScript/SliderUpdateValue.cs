using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderUpdateValue : MonoBehaviour {
    public Text textToUpdate;
    
	public void updateValue(Slider s)
    {
        Debug.Log(Mathf.RoundToInt(s.value));
        string test = Mathf.RoundToInt(s.value).ToString();
        textToUpdate.text = test + "%";

        // mettre les éléments du controle de la musique général
    }

}
