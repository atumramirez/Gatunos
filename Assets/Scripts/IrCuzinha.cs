using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IrCuzinha : MonoBehaviour
{
    public Button setaDireita;
    public Button setaEsquerda;
    public Button setaDireitaRestaurante;
    public Button setaEsquerdaArmario;
    public Button start;

    public GameObject meio;
    public GameObject cuzinha;
    public GameObject balcao;
    public GameObject restaurante;
    public GameObject armario;

    public GameObject balcaoButoes;
    public GameObject cuzinhasButoes;
    public GameObject restauranteButoes;
    public GameObject armarioButoes;
    public GameObject finalMau;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip fadeSound;

    void Start()
    {
        setaDireita.onClick.AddListener(() => TransitionTo(AbrirCuzinha));
        setaEsquerda.onClick.AddListener(() => TransitionTo(AbrirBalcao));
        setaDireitaRestaurante.onClick.AddListener(() => TransitionTo(AbrirArmario));
        setaEsquerdaArmario.onClick.AddListener(() => TransitionTo(AbrirRestaurante));
        start.onClick.AddListener(() => TransitionTo(AbrirBalcao));

        TransitionTo(AbrirRestaurante);
    }

    public void TransitionTo(System.Action sceneSetup)
    {
        StartCoroutine(PlayTransition(sceneSetup));
    }

    IEnumerator PlayTransition(System.Action sceneSetup)
    {
  
        if (fadeSound && audioSource)
        {
            audioSource.PlayOneShot(fadeSound);
        }

        yield return StartCoroutine(FadeScreen(0f, 1f)); 

        sceneSetup?.Invoke(); 

        yield return new WaitForSeconds(0.2f); 

        yield return StartCoroutine(FadeScreen(1f, 0f));
    }

    IEnumerator FadeScreen(float fromAlpha, float toAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, toAlpha);
    }

    public void AbrirCuzinha()
    {
        meio.SetActive(true);
        cuzinha.SetActive(true);
        cuzinhasButoes.SetActive(true);

        balcao.SetActive(false);
        balcaoButoes.SetActive(false);
        restaurante.SetActive(false);
        restauranteButoes.SetActive(false);
        armario.SetActive(false);
        armarioButoes.SetActive(false);

        finalMau.SetActive(false);
    }

    void AbrirBalcao()
    {
        meio.SetActive(true);
        cuzinha.SetActive(false);
        cuzinhasButoes.SetActive(false);

        balcao.SetActive(true);
        balcaoButoes.SetActive(true);
        restaurante.SetActive(false);
        restauranteButoes.SetActive(false);
        armario.SetActive(false);
        armarioButoes.SetActive(false);

        finalMau.SetActive(false);
    }

    public void AbrirRestaurante()
    {
        meio.SetActive(false);
        cuzinha.SetActive(false);
        cuzinhasButoes.SetActive(false);

        balcao.SetActive(false);
        balcaoButoes.SetActive(false);
        restaurante.SetActive(true);
        restauranteButoes.SetActive(true);
        armario.SetActive(false);
        armarioButoes.SetActive(false);

        finalMau.SetActive(false);
    }

    void AbrirArmario()
    {
        meio.SetActive(false);
        cuzinha.SetActive(false);
        cuzinhasButoes.SetActive(false);

        balcao.SetActive(false);
        balcaoButoes.SetActive(false);
        restaurante.SetActive(false);
        restauranteButoes.SetActive(false);
        armario.SetActive(true);
        armarioButoes.SetActive(true);

        finalMau.SetActive(false);
    }

    public void MostrarFinalMau()
    {
        meio.SetActive(false);
        cuzinha.SetActive(false);
        cuzinhasButoes.SetActive(false);

        balcao.SetActive(false);
        balcaoButoes.SetActive(false);
        restaurante.SetActive(false);
        restauranteButoes.SetActive(false);
        armario.SetActive(false);
        armarioButoes.SetActive(false);

        finalMau.SetActive(true);
    }
}
