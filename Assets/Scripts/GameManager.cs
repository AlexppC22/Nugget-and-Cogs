using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("> Referências")]
    public static GameManager instance;
    [SerializeField, Tooltip("Texto do mascote")] Text nuggetText;
    [SerializeField] GameObject[] cogs;

    [Header("> Gameplay Vars")]
    [HideInInspector] public int cogsInPlace;
    [HideInInspector] public bool canSpin;
    private void Awake()
    {
        //Usado de referencia em outros Scripts
        instance = this;
    }
    public void CheckWin() //Verifica se todas as engrenagens estão no local correto e muda o texto do Nugget de acordo
    {
        //Verifica se todas as engrenagens estão no local correto
        //Muda o texto do Nugget de acordo e ajusta a rotação das engrenagens
        if (cogsInPlace >= 5)
        {
            canSpin = true;
            nuggetText.text = $"YAY, PARABÉNS. TASK CONCLUÍDA!";
        }
        else
        {
            canSpin = false;
            nuggetText.text = $"ENCAIXE AS ENGRENAGENS EM QUALQUER ORDEM";
        }
    }
    public void RestartButton() //Responsavel por reiniciar o jogo
    {
        for(int i = 0; i < cogs.Length; i++)
        {
            cogs[i].GetComponent<CogController>().SetState(CogState.restart);
            cogsInPlace = 0;
            CheckWin();
        }
    }
    
}
