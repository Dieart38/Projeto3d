using UnityEngine;

public class SlimeA : MonoBehaviour
{
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Meus Metodos

    void GetHit()
    {
        anim.SetTrigger("GetHit");
    }

    #endregion
}
