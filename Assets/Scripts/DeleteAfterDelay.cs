using System.Collections;
using UnityEngine;

public class DeleteAfterDelay : MonoBehaviour
{
    public GameObject targetObject; 

    public void OnButtonClick()
    {
        StartCoroutine(DeleteObjectAfterDelay(5f));
    }

    private IEnumerator DeleteObjectAfterDelay(float delay)
    {
  
        yield return new WaitForSeconds(delay);

        Destroy(targetObject);
    }
}
