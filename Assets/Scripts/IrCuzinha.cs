using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class IrCuzinha : MonoBehaviour
{
    public Button setaDireita;
    public Button setaEsquerda;
    public GameObject cuzinha;
    public GameObject balcao;
    public GameObject balcaoButoes;
    public GameObject cuzinhasButoes;

    void Start()
    {
        setaDireita.onClick.AddListener(() => AbrirCuzinha());
        setaEsquerda.onClick.AddListener(() => AbrirBalcao());
    }

    void AbrirCuzinha()
    {
        cuzinha.SetActive(true);
        cuzinhasButoes.SetActive(true);
        balcao.SetActive(false);
        balcaoButoes.SetActive(false);
    }

    void AbrirBalcao()
    {
        cuzinha.SetActive(false);
        cuzinhasButoes.SetActive(false);
        balcao.SetActive(true);
        balcaoButoes.SetActive(true);
    }
}
