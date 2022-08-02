using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _textChar;
    [SerializeField] private GameObject inputField;
    [SerializeField] private EventsManager EventsManager;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private Image inputIMG;
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private TextMeshProUGUI attemptsText;
    [SerializeField] private GameObject dubText;
    private string randomWord;
    private string userInput;
    private int attempts;
    
    [SerializeField]private List<string> wordList;

    private void Awake()
    {
        EventsManager = FindObjectOfType<EventsManager>().GetComponent<EventsManager>();
    }
    private void OnEnable()
    {
        EventsManager.StartGame += ShowUI;
    }
    private void OnDisable()
    {
        EventsManager.StartGame -= ShowUI;
    }
    private void ShowUI()
    {
        inputField.SetActive(true);
        rulesPanel.SetActive(true);
        rulesPanel.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 1f, 1, 0.2f).SetEase(Ease.Flash);
        inputField.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 1f, 1, 0.2f).SetEase(Ease.InOutBounce);
    }
    private void GenerateWordList()
    {
        //Thanks for the help Josh
        wordList = textFile.ToString().Split("\n").ToList();
    }
    private string GrabRandomWord()
    {
        var word = wordList[Random.Range(0, wordList.Count)];
        //Debug.Log(randomWord);

        return word;
    }
    public void RetrieveInput(string s)
    {
        userInput = s.ToLower();
        Debug.Log(userInput);
        GameHandler();
    }
    private void AnimateWrong()
    {
        Color ogColor = inputIMG.color;
       // inputField.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.1f, 1, 0.1f).SetEase(Ease.InElastic);
       inputField.transform.DOShakePosition(0.1f, new Vector3(4f, 4f), 1).SetEase(Ease.InBounce);
       inputIMG.DOColor(Color.red, 0.3f).SetEase(Ease.OutBounce).OnComplete((() => inputIMG.color = ogColor));
    }
    private void AnimateRight()
    {
        Color ogColor = inputIMG.color;
        // inputField.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.1f, 1, 0.1f).SetEase(Ease.InElastic);
        inputField.transform.DOShakePosition(0.1f, new Vector3(4f, 4f), 1).SetEase(Ease.InBounce);
        inputIMG.DOColor(Color.green, 0.3f).SetEase(Ease.OutBounce).OnComplete((() => inputIMG.color = ogColor));
    }

    /// <summary>
    /// Grabs the "guess" and checks to see if it's a valid word in the list.
    /// </summary>
    /// <param name="guess"></param>
    /// <returns></returns>
    private bool IsRealWord(string guess)
    {
       return (wordList.Contains(guess));
    }
    private void UpdateAttempts()
    {
        attempts++;
        attemptsText.text = $"Attempts: {attempts}";

    }
    private void GameHandler()
    {
        if (IsRealWord(userInput))
        {
            UpdateAttempts();
            var checkedString = CheckGuess(userInput, randomWord); //Lol couldn't use to the full extent...sadge.
        }
        else
        {
            AnimateWrong();
            UpdateAttempts();
            //return;
        }
        
        
        if (userInput != randomWord)
        {
            if (attempts > 2)
            {
                ResetGame(false);
            }
        }
        else
        {
            AnimateRight();
            dubText.SetActive(true);
            dubText.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 1f, 1, 0.2f).SetEase(Ease.InBounce).OnComplete((() => dubText.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBounce).OnComplete((() => dubText.SetActive(false)))));
            ResetGame(true);
        }
        
        
    }
    private void ResetGame(bool win)
    {
        switch (win)
        {
            case true:
                Debug.Log("Congrats!");
                break;
            case false:
                Debug.Log($"Try again, the word was {randomWord}!");
                break;
        }

        attempts = 0;
        randomWord = GrabRandomWord();
        Debug.Log(randomWord);
        EventsManager.OnWipe();
    }
    

    private string CheckGuess(string guess, string rdWord)
    {
       
        StringBuilder outString = new StringBuilder(5);
        List<char> usedChar = new List<char>();
        
        
        for (int i = 0; i < rdWord.Length; i++)
        {
            if (rdWord.Contains(guess[i]))//Character is in word
            {
                if (rdWord[i] == guess[i]) //Correct position
                {
                    outString.Append('X');
                    _textChar[i].text = rdWord[i].ToString();
                }
                else //Exists but not in correct position
                {
                    if (usedChar.Contains(guess[i]))
                    {
                        outString.Append('-');
                        _textChar[i].text = "-";
                        
                    }
                    else
                    {
                        outString.Append('+');
                        _textChar[i].text = "+";     
                    }
                }
            }
            else //If it doesnt exist
            {
                outString.Append('-');
                _textChar[i].text = "-";
            }
            
            if (!usedChar.Contains(guess[i]))
            {
                usedChar.Add(guess[i]);
            }
        }
        return outString.ToString();
    }


    private void Start()
    {
        GenerateWordList();
        randomWord = GrabRandomWord();
        Debug.Log(randomWord);
       
    }
    
    
}
