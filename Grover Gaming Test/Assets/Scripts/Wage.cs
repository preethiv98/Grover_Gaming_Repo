using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Wage : MonoBehaviour
{
    [Header("Denomination Inputs/Buttons")]
    public Button upArrow;
    public Button downArrow;
    public Button playButton;
    public TMP_Text denominationText;

    [Header("Readouts/Win Amounts")]
    public TMP_Text wonAmount;
    public TMP_Text balanceReadout;
    public TMP_Text lastBalanceReadoutText;
    public float lastBalanceReadOut = 0;

    public GameObject pooper;

    [Header("Audio")]
    AudioSource source;
    public AudioClip drumroll;
    public AudioClip win;
    public AudioClip aww;

    public static bool alreadyClicked = false;

    public float currentBalanceReadOut = 10f; //starting balance

    float currentDenomination; //the current denomination player is on
    int count = 0; //index of denomination

    [Header("Multipliers/Denomination List")]
    public float[] denomination = {0.25f, 0.50f,1.00f,5.00f};
    public List<int> thirty = new List<int>
    {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    public List<int> fifteen = new List<int>
    {12, 16, 24, 32, 48, 64};
    public List<int> five = new List<int>
    {100, 200, 300, 400, 500, 600};

    public List<GameObject> chests; //contains all the chest buttons in game

    public List<float> chestValues; //value of chests without randomizing
    public List<float> finishedChestValues; //value of chests after randomizing

    public List<float> percentValues;

    public GameObject won;


 



    int numberOfChests = 8; //number of chests that the pot is split into

    void Start()
    {
        source = GetComponent<AudioSource>();
        denominationText.text = denomination[0].ToString();
        currentDenomination = denomination[0];
        balanceReadout.text = currentBalanceReadOut.ToString();
        foreach(GameObject g in chests)
        {
            g.GetComponent<Button>().interactable = false;
        }
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RaiseDenomination()
    {
        if(count == denomination.Length-1)
        {
            count = 0;
        }
        else
        {
            count++;
        }

        if (currentDenomination > currentBalanceReadOut)
        {
            playButton.interactable = false;
        }
        else
        {
            playButton.interactable = true;
        }
        currentDenomination = denomination[count];
        denominationText.text = denomination[count].ToString();
    }

    public void LowerDenomination()
    {
        if (count == 0)
        {
            count = denomination.Length-1;
        }
        else
        {
            count--;
        }

        if(currentDenomination > currentBalanceReadOut)
        {
            playButton.interactable = false;
        }
        else
        {
            playButton.interactable = true;
        }
        currentDenomination = denomination[count];
        denominationText.text = denomination[count].ToString();
    }


    public void Play()
    {

        /*
         * Start the playing process. Get a random multiplier and distribute the winning pot over
         * several chests.
         * 
         * 
         * 
         * 
         */
        GameObject[] chestList = GameObject.FindGameObjectsWithTag("Chest");
        foreach (GameObject g in chestList)
        {
            g.GetComponent<Animator>().Play("TreasureClose"); //play closing animation for chests
        } 
        /*
         * Disable buttons for play
         */
        upArrow.interactable = false;
        downArrow.interactable = false;
        playButton.interactable = false;
        pooper.SetActive(false);
        
        int multiplier;
        foreach (GameObject g in chests)
        {
            g.GetComponent<Button>().interactable = true;
        }
        currentBalanceReadOut -= currentDenomination; //subtract denomination from balance
        balanceReadout.text = currentBalanceReadOut.ToString(); //read the current balance to text.
        
        float value = Random.value; //get a random percent for the multipliers
        if(value > 0.95) //five percent chance of getting highest multiplier
        {
            int index = Random.Range(0, five.Count);//fetch a random multiplier from the 5% list
            multiplier = five[index];
        }
        else if (Random.value > 0.85)//fifteen percent chance of getting highest multiplier
        {
            int index = Random.Range(0, five.Count);//fetch a random multiplier from the 15% list
            multiplier = fifteen[index];
        }
        else if(value > 0.7)//thirty percent chance of getting highest multiplier
        {
            int index = Random.Range(0, five.Count);//fetch a random multiplier from the 30% list
            multiplier = thirty[index];
        }
       
       else //50% chance of getting a zero.
        {
            multiplier = 0;
        }

        float totalAmount = currentDenomination * multiplier; //total amount in the chest
        int numberofChests = Random.Range(1, 9); //number of chests the total amount will be distributed
        float stepSize = 0.200000001f; //decided to do percent intervals of 0.2 
        int totalChests = 9; //total of all chests that can be clicked

        float startingValue = 1f; // 100% starting value
        totalChests -= numberOfChests; //for adding 0s once the number of chests value runs out


        if (multiplier == 0) //just add zero if the multiplier is zero
        {
            chestValues.Add(0);
        }
        else
        {
            /*
             * 
             * Loops through the chests that will contain the total win. The starting value will be split into percentage
             * values that will be multiplied by the totalAmount to get the value in the specific chest. Once all the percentage
             * values are split into each chest, it will add 0 to any remaining chests that are not accoutned for.
             * 
             * The money you can win are NOT tied to each chest, they are predetermined. In other words, I have already split the chest wins
             * accordingly in the list, what chest you pick does not matter, giving the "random game" vibe.
             */
            for(int i = 1; i <= numberofChests; i++)
            {
                    if(startingValue > 0)
                    {
                        float chestValue = totalAmount * startingValue;
                        if (numberofChests == 1)
                        {
                            chestValues.Add(chestValue);
                            break;
                        }
                        /*
                         * Gets a random percentage and floors the value in intervals of 20%. Multiply that value with the total amount
                         * to get the chest win.
                         * 
                         */
                        float startingValue2 = Random.Range(0f, startingValue); 
                        float numValues = Mathf.Floor(startingValue2 / stepSize);
                        if (startingValue2 == 0)
                        {
                            startingValue2 = 0.2f;
                        }
                        float adjustedValue = numValues * stepSize;
                        if (adjustedValue < 0.2)
                        {
                            adjustedValue = 0.2f;
                        }
                        percentValues.Add(adjustedValue);
                       
                        
                        chestValue = adjustedValue * totalAmount;
                        chestValues.Add(chestValue);
                        
                            startingValue -= adjustedValue;
                       
                    }
                    else
                    {
                        chestValues.Add(0);
                    }
                   

              
                
            }
            while (totalChests != 0)
            {
                chestValues.Add(0);
                totalChests--;
            }
          

        }
        percentValues.Clear(); //clear percent value list
        //GenerateRandomList();

        for (int i = 0; i < chests.Count; i++)
        {
            chests[i].gameObject.GetComponent<Button>().interactable = true; //make all the chests playable
        }
       

        
    }

    public void GenerateRandomList() //randomizes the amount of money in each chest
    {
       
        while (chestValues.Count != 0)
        {
            float ranNum = chestValues[Random.Range(0, chestValues.Count)];
            finishedChestValues.Add(ranNum);
            
            chestValues.Remove(ranNum);

        }
       
    }


    public void OpenChest() //Delay for sound effects in opening the chest
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        won.SetActive(false);
        source.PlayOneShot(drumroll, 0.4f);
        if (!alreadyClicked)
        {
            alreadyClicked = true;
            Debug.Log("Drumroll!");
            Invoke("ChestResults", 4f);
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().Play("TreasureOpen"); //chest opens and makes it no longer interactable until turn is over
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        }


    }

    public void ChestResults()
    {
       
        won.SetActive(true);
        int rand = Random.Range(0, chestValues.Count - 1);
        if (chestValues[rand] == 0) //if the chest turns out to be the pooper, show the pooper text and end the playing round
        {
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            
            source.PlayOneShot(aww, 0.6f);
            won.SetActive(false);
            alreadyClicked = false;
            pooper.SetActive(true);
            currentBalanceReadOut += lastBalanceReadOut;
            balanceReadout.text = currentBalanceReadOut.ToString();
            lastBalanceReadOut = 0;
            lastBalanceReadoutText.text = lastBalanceReadOut.ToString();
            chestValues.Clear();
           
            foreach (GameObject g in chests)
            {
                g.GetComponent<Button>().interactable = false; //don't make them interactable unless play button is clicked again
            }
            upArrow.interactable = true;
            downArrow.interactable = true;
            //playButton.interactable = true;
            if (currentDenomination > currentBalanceReadOut)
            {
                playButton.interactable = false;
            }
            else
            {
                playButton.interactable = true;
            }


            return;
        }
        source.PlayOneShot(win, 0.4f);
        //Add the total won amount to the last game win read out and remove the value from the chest.
        lastBalanceReadOut += chestValues[rand];
        lastBalanceReadoutText.text = lastBalanceReadOut.ToString();
        alreadyClicked = false;
        wonAmount.text = chestValues[rand].ToString();
        chestValues.RemoveAt(rand);
        Cursor.lockState = CursorLockMode.None;
       // Cursor.visible = true;
    }

}
