using UnityEngine;
using DG.Tweening; 

[RequireComponent(typeof(MeshRenderer))] 
[RequireComponent(typeof(AudioSource))]
public class BoxEffect : MonoBehaviour
{
    [Header("Effect Components")]
    public ParticleSystem popParticles;
    public AudioClip popSound;

    [Header("Animation Settings")]
    public float popScale = 1.2f;
    public float popDuration = 0.6f;
    public float fadeDuration = 0.15f;

    private MeshRenderer meshRenderer;
    private AudioSource audioSource;
    private bool isHit = false;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        popParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void OnHit()
    {
        if (isHit) return;
        isHit = true;

        // --- 1. The Scale Tween (This is the same) ---
        transform.DOScale(popScale, popDuration).SetEase(Ease.OutBack);

        // --- 2. The Fade Tween (This part is different) ---
        meshRenderer.material.DOFade(0f, fadeDuration).SetEase(Ease.InSine);

        // --- 3. The Particle Burst ---
        if (popParticles != null)
        {
            popParticles.Play();
        }

        // --- 4. The Sound ---
        if (popSound != null)
        {
            audioSource.PlayOneShot(popSound);
        }
        // --- 5. Destroy the Box ---
        Destroy(gameObject, popDuration);
    }
    public void SetParticleColor(Color color)
    {
        if (popParticles != null)
        {
            var main = popParticles.main;
            main.startColor = color;
        }
    }
}