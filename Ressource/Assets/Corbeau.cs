using System.Collections;
using UnityEngine;

public class Corbeau : MonoBehaviour
{
    [SerializeField] private Transform _corbeau;
    
    void OnEnable()
    {
        StartCoroutine(PrincipalAnima());
    }

    private IEnumerator PrincipalAnima()
    {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(FaitGrossirLeCorbeau());
    }

    private IEnumerator FaitGrossirLeCorbeau()
    {
       var maxScale = new Vector3(10, 10, 10);
       var t = 0f;
        
        while (_corbeau.localScale.x < 9.99f)
        {
            yield return new WaitForEndOfFrame();
            _corbeau.localScale = Vector3.Lerp(_corbeau.localScale, maxScale, t);
            t += Time.deltaTime;
        }
    }
}
