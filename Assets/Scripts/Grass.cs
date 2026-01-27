using UnityEngine;
using System.Collections;

public class Grass : MonoBehaviour
{   
    [Header("Configurações de Efeito")]
    public ParticleSystem fxHit;
    public float respawnTime = 5.0f;
    
    [Header("Balanço (Sway)")]
    public float swaySpeed = 2.0f;    // Velocidade do balanço
    public float swayAmount = 5.0f;   // Ângulo máximo de inclinação
    
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private bool isCut;

    void Start()
    {   
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Só balança se a grama não estiver cortada
        if (!isCut)
        {
            ApplySway();
        }
    }

    void ApplySway()
    {
        // Calcula a inclinação usando Seno com base no tempo
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        
        // Aplica a rotação apenas no eixo Z (ou X, dependendo do seu modelo)
        transform.rotation = initialRotation * Quaternion.Euler(sway, 0, sway);
    }

    void GetHit(int amount)
    {
        if (!isCut)
        {
            isCut = true;
            CutGrass();
            StartCoroutine(RegrowGrass());
        }
    }

    void CutGrass()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        // 
        if (fxHit != null) fxHit.Emit(20);
    }

    IEnumerator RegrowGrass()
    {
        yield return new WaitForSeconds(respawnTime);

        float timer = 0;
        float growDuration = 0.8f;

        while (timer < growDuration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(new Vector3(0.2f, 0.2f, 0.2f), initialScale, timer / growDuration);
            yield return null;
        }

        transform.localScale = initialScale;
        isCut = false;
    }
}