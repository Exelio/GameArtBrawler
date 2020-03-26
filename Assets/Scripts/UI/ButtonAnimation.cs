using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    
    public void Pressed()
    {
        this.transform.parent.GetComponent<Animator>().SetBool("Pressed", true );
        this.transform.parent.GetComponent<Animator>().SetBool("Deselected", true );
    }

    public void Selected()
    {
        this.transform.parent.GetComponent<Animator>().SetBool("Selected", true);
        this.transform.parent.GetComponent<Animator>().SetBool("Pressed", false);

    }

    public void DeSelected()
    {
        this.transform.parent.GetComponent<Animator>().SetBool("Selected", false);

    }

    void ButtonIsPressed()
    {
        Debug.Log("Works");
    }
}
