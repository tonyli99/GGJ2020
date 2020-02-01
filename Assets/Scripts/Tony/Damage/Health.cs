using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float hitPoints = 10;
    public float damageFlashDuration = 1;
    public Color colorToFlash;
    public GameObject deathPrefab;

    private SpriteRenderer spriteRenderer;
    private Shader flashShader;
    private Shader originalShader = null; 

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        flashShader = Shader.Find("GUI/Text Shader");
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
            if (deathPrefab != null) Instantiate(deathPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Flash();
        }
    }

    void Flash()
    {
        if (spriteRenderer == null) return;
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        if (spriteRenderer == null) yield break;
        if (originalShader == null) originalShader = spriteRenderer.material.shader;
        spriteRenderer.material.shader = flashShader;
        spriteRenderer.color = colorToFlash;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.material.shader = originalShader;
        spriteRenderer.color = Color.white;
    }
}
