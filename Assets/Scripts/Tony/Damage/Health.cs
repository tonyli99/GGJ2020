using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float hitPoints = 10;
    public float damageFlashDuration = 1;
    public Color colorToFlash = Color.red;
    public GameObject deathPrefab;

    private SpriteRenderer[] spriteRenderers;
    private Shader flashShader;
    private Shader originalShader = null; 

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
        if (spriteRenderers.Length == 0) return;
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        if (spriteRenderers.Length == 0) yield break;
        if (originalShader == null) originalShader = spriteRenderers[0].material.shader;
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].material.shader = flashShader;
            spriteRenderers[i].color = colorToFlash;
        }
        yield return new WaitForSeconds(damageFlashDuration);
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].material.shader = originalShader;
            spriteRenderers[i].color = Color.white;
        }
    }
}
