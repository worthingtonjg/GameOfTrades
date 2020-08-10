using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static int TurnsRemaining = 0;
    public static EnumRegion? SelectedRegion;
    public static int GoldMultiplier = 1;
    public static int TotalGold = 0;
    public static float startTime;
    public static EnumGameState GameState = EnumGameState.SpinningWheel;
    public static int RetireEveryXTurns = 5;
    public static int TurnsSinceRetirePrompt = 0;
    public static int StartingTurns = 25;

    public AudioSource audioSource;
    public AudioClip moneyClip;
    public AudioClip failClip;
    public AudioClip winClip;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI RewardText;
    public Image progress;
    public GameObject CardPanel;
    public GameObject RetirePanel;
    public GameObject GameOverPanel;
    public GameObject SpinCanvas;
    public GameObject SpinWheel;
    public Image CardImage;
    public List<Sprite> Images;

    public float turnLength = 30f;

    private List<Card> Cards = new List<Card>();
    private Card selectedCard = null;

    private Spin spinComponent;

    // Start is called before the first frame update
    void Start()
    {
        GameController.TurnsRemaining = StartingTurns;
        GameController.SelectedRegion = null;
        GameController.GoldMultiplier = 1;
        GameController.TotalGold = 0;
        GameController.GameState = EnumGameState.SpinningWheel;

        Cards = Card.SetupCards();
        spinComponent = SpinWheel.GetComponent<Spin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameState == EnumGameState.InTurn) 
        {
            if(Time.time - startTime >= turnLength)
            {
                GameState = EnumGameState.BetweenTurns;

                var cardsForRegion = Cards.Where(c => c.Region == SelectedRegion).ToList();
                var cardIndex = Random.Range(0,cardsForRegion.Count);
                selectedCard = cardsForRegion[cardIndex];

                DescriptionText.text = selectedCard.Text;
                RewardText.text = selectedCard.GetReward(GoldMultiplier);

                var imageName = $"m{(int)SelectedRegion}0{cardIndex+1}";
                print(imageName);

                var image = Images.Where(i => i.name.Contains(imageName)).FirstOrDefault();
                if(image != null)
                {
                    CardImage.sprite = image;
                }

                CardPanel.SetActive(true);

                string turns = selectedCard.GetReward(GoldMultiplier, EnumRewardFormat.Turns);
                string gold = selectedCard.GetReward(GoldMultiplier, EnumRewardFormat.Gold);

                if((!string.IsNullOrEmpty(turns) && turns.Contains("+")) || (!string.IsNullOrEmpty(gold) && gold.Contains("+")))
                {
                    audioSource.PlayOneShot(winClip);
                }
                else
                {
                    audioSource.PlayOneShot(failClip);
                }

            }
            else
            {
                progress.fillAmount =  (Time.time - startTime) / turnLength;
            }
        }
    }

    public void AddGold(int amount)
    {
        if(GameState != EnumGameState.InTurn) return;

        TotalGold += (amount * GoldMultiplier);
        UpdateGold();

        audioSource.PlayOneShot(moneyClip);
    }

    public void NextTurn()
    {
        string turns = selectedCard.GetReward(GoldMultiplier, EnumRewardFormat.Turns);
        string gold = selectedCard.GetReward(GoldMultiplier, EnumRewardFormat.Gold);

        print($"turns: {turns}");
        print($"gold: {gold}");

        if(!string.IsNullOrEmpty(turns))
        {
            if(turns.StartsWith("-"))
            {
                TurnsRemaining -= int.Parse(turns.Substring(1));
            }
            else
            {
                TurnsRemaining += int.Parse(turns.Substring(1));
            }
        }

        if(!string.IsNullOrEmpty(gold))
        {
            if(gold.StartsWith("-"))
            {
                if(gold.Contains(".5"))
                {
                    TotalGold = TotalGold / 2;
                }
                else if(gold.Contains("*"))
                {
                    TotalGold = 0;
                }
                else
                {
                    TotalGold -= int.Parse(gold.Substring(1));
                }
            }
            else
            {
                TotalGold += int.Parse(gold.Substring(1)) * GoldMultiplier;
            }

            UpdateGold();
        }
    
        if(TurnsRemaining <= 0)
        {
            GameOverPanel.SetActive(true);
        }

        if(TurnsSinceRetirePrompt >= RetireEveryXTurns)
        {
            ShowRetirePanel();
            return;
        }

        ShowSpinCanvas();
    }

    private void ShowRetirePanel()
    {
        TurnsSinceRetirePrompt = 0;
        GameState = EnumGameState.ContinueOrRetire;
        CardPanel.SetActive(false);
        RetirePanel.SetActive(true);
    }

    public void Retire()
    {
        RetirePanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }

    public void StartOver()
    {
        SceneManager.LoadScene("01StartMenu");
    }

    public void ShowSpinCanvas()
    {
        GameState = EnumGameState.SpinningWheel;
        --GameController.TurnsRemaining;
        spinComponent.Reset();
        CardPanel.SetActive(false);
        RetirePanel.SetActive(false);
        SpinCanvas.SetActive(true);
    }

    private void UpdateGold()
    {
        GoldText.text = TotalGold.ToString();
    }
}
