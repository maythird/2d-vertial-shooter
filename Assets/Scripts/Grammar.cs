using UnityEngine;
using UnityEngine.UI;
public class Grammar : MonoBehaviour
{
    public Button btnObj;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        
        btnObj.onClick.AddListener(() =>
            {
                
            }
        );
    }

    public void Test()
    {
        Debug.Log("test");
    }
   
}
