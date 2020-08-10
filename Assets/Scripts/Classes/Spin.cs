using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Spin : MonoBehaviour
{
    public RectTransform rect;
    public AudioClip spinClip;
    public AudioClip winClip;
    public AudioClip failClip;
    public AudioSource audioSource;
    public GameObject instructions;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI turnsRemainingText;
    public TextMeshProUGUI selectedVentureText;
    public GameObject continueButton;
    public GameObject spinCanvas;
    public GameObject chooseNextPanel;
    public GameObject clickForGold;
    public GameObject turnProgress;
    public TMP_Dropdown selectedVenture;
    public List<GameObject> Backgrounds;

    private bool canSpin = true;
    private bool spinAgain = false;
    private bool isSpinning;
    private float rotationSpeed = 1;
    private float startTime;
    private EnumSpinValues? result;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
        selectedVentureText.text = "";
        audioSource.clip = spinClip;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(isSpinning)
        {
            var percentComplete = (Time.time - startTime) / spinClip.length;
            rect.Rotate(0 , 0, rotationSpeed - (rotationSpeed * percentComplete), Space.World);
            if(!audioSource.isPlaying)
            {
                isSpinning = false;

                var spinValues = System.Enum.GetNames(typeof(EnumSpinValues));
                
                result = (EnumSpinValues)Random.Range(0, spinValues.Length);

                switch(result)
                {
                    case EnumSpinValues.IndianOcean : 
                        instructionText.text = GetVentureText();
                        GameController.SelectedRegion = EnumRegion.IndianOcean;
                        break;
                    case EnumSpinValues.StraitOfMalacca : 
                        instructionText.text = GetVentureText();
                        GameController.SelectedRegion = EnumRegion.StraitOfMalacca;
                        break;
                    case EnumSpinValues.MediterraneanSea : 
                        instructionText.text = GetVentureText();
                        GameController.SelectedRegion = EnumRegion.MediterraneanSea;
                        break;
                    case EnumSpinValues.SaharaDesert : 
                        instructionText.text = GetVentureText();
                        GameController.SelectedRegion = EnumRegion.SaharaDesert;
                        break;
                    case EnumSpinValues.YourChoice : 
                        audioSource.PlayOneShot(winClip);
                        instructionText.text = "You get to choose your next venture!";
                        break;
                    case EnumSpinValues.LoseATurn : 
                        audioSource.PlayOneShot(failClip);
                        instructionText.text = "You lose a turn";
                        spinAgain = true;
                        break;
                    case EnumSpinValues.GainATurn : 
                        audioSource.PlayOneShot(winClip);
                        instructionText.text = "You gain a turn";
                        spinAgain = true;
                        break;
                    case EnumSpinValues.DoubleGoldEarned : 
                        audioSource.PlayOneShot(winClip);
                        instructionText.text = "All gold earned is doubled";
                        spinAgain = true;
                        break;
                }

                instructions.SetActive(true);
                continueButton.SetActive(true);
            }
        }
    }
    
    public void Continue()
    {
        if(spinAgain)
        {
            if(result == EnumSpinValues.GainATurn)
            {
                ++GameController.TurnsRemaining;
                UpdateTurnsRemaining();
            }

            if(result == EnumSpinValues.LoseATurn)
            {
                --GameController.TurnsRemaining;
                UpdateTurnsRemaining();
            }

            if(result == EnumSpinValues.DoubleGoldEarned)
            {
                if(GameController.GoldMultiplier == 1)
                {
                    ++GameController.GoldMultiplier;
                }
                else
                {
                    GameController.GoldMultiplier *= 2;
                }
            }
            
            Reset();
        }
        else
        {
            if(GameController.SelectedRegion == null)
            {
                ChooseNextVenture();
            }
            else
            {
                StartVenture();
            }
        }
    }

    public void ChooseNextVenture()
    {
        instructions.SetActive(false);
        chooseNextPanel.SetActive(true);
    }

    public void StartVenture()
    {
        if(GameController.SelectedRegion == null)
        {
            result = (EnumSpinValues)selectedVenture.value;
            GameController.SelectedRegion = (EnumRegion)selectedVenture.value;
        }

        HideBackgrounds();

        Backgrounds[(int)GameController.SelectedRegion].SetActive(true);
        spinCanvas.SetActive(false);
        clickForGold.SetActive(true);
        turnProgress.SetActive(true);

        selectedVentureText.text = GetVentureText();
        GameController.startTime = Time.time;
        GameController.GameState = EnumGameState.InTurn;
        ++GameController.TurnsSinceRetirePrompt;
    }

    public void Reset()
    {
        selectedVentureText.text = "";
        result = null;
        HideBackgrounds();
        clickForGold.SetActive(false);
        turnProgress.SetActive(false);
        chooseNextPanel.SetActive(false);
        continueButton.SetActive(false);
        instructionText.text = "Click to Spin";
        instructions.SetActive(true);
        canSpin = true;
        spinAgain = false;
        GameController.SelectedRegion = null;
        UpdateTurnsRemaining();
    }

    public void SpinWheel()
    {
        if(isSpinning || !canSpin) return;
        rotationSpeed = Random.Range(1f, 3f);
        instructions.SetActive(false);

        startTime = Time.time;
        audioSource.Play();

        isSpinning = true;
        canSpin = false;
    }

    private void UpdateTurnsRemaining()
    {
        turnsRemainingText.text = GameController.TurnsRemaining.ToString();
    }

    private string GetVentureText()
    {
        switch(result)
        {
            case EnumSpinValues.IndianOcean : 
                return "Indian Ocean";
            case EnumSpinValues.StraitOfMalacca : 
                return "Strait Of Malacca";
            case EnumSpinValues.MediterraneanSea : 
                return "Mediterranean Sea";
            case EnumSpinValues.SaharaDesert : 
                return "Sahara Desert";
        }

        return string.Empty;
    }

    private void HideBackgrounds()
    {
        Backgrounds.ForEach(b => {
            b.SetActive(false);
        });
    }
}
