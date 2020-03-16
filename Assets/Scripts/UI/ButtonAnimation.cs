using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    
    public void Pressed()
    {
        this.transform.parent.GetComponent<Animator>().SetBool("Pressed", true );
    }

    public void Selected()
    {
        this.transform.parent.GetComponent<Animator>().SetBool("Selected", true);
    }

    public void DeSelected()
    {
        this.transform.parent.GetComponent<Animator>().SetBool("Selected", false);

    }
}
