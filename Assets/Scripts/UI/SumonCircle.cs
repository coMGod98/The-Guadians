using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SumonCircle : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
   public Animator animator;
   public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("OnClick", true);
    }
   public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool("OnClick", false);

    }
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
