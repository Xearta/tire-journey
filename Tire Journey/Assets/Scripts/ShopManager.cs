using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int currentCharacterIndex;
    public GameObject[] characterModels;

    public ShopElement[] characters;
    public Button buyButton;
    public Button playButton;

    public Text coinsText;

    // Start is called before the first frame update
    void Start()
    {
        // Check which cars are locked (by default, locked)
        foreach(ShopElement ch in characters)
        {
            if (ch.price != 0)
                ch.isLocked = PlayerPrefs.GetInt(ch.name, 1) == 1 ? true : false;
        }

        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        UpdateUI();

        foreach(GameObject character in characterModels)
        {
            character.SetActive(false);
        }

        characterModels[currentCharacterIndex].SetActive(true);
    }

    public void ChangeNextCharacter()
    {
        characterModels[currentCharacterIndex].SetActive(false);

        currentCharacterIndex++;
        if (currentCharacterIndex == characterModels.Length)
            currentCharacterIndex = 0;

        characterModels[currentCharacterIndex].SetActive(true);

        UpdateUI();

        bool isLocked = characters[currentCharacterIndex].isLocked;
        if (isLocked)
            return;

        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
    }

    public void ChangePreviousCharacter()
    {
        characterModels[currentCharacterIndex].SetActive(false);

        currentCharacterIndex--;
        if (currentCharacterIndex == -1)
            currentCharacterIndex = characterModels.Length - 1;

        characterModels[currentCharacterIndex].SetActive(true);

        UpdateUI();

        bool isLocked = characters[currentCharacterIndex].isLocked;
        if (isLocked)
            return;

        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
    }


    public void UnlockWithCoins()
    {
        ShopElement ch = characters[currentCharacterIndex];
        if (PlayerPrefs.GetInt("TotalCoins", 0) < ch.price)
            return;

        

        int newCoins = PlayerPrefs.GetInt("TotalCoins", 0) - characters[currentCharacterIndex].price;
        PlayerPrefs.SetInt("TotalCoins", newCoins);

        ch.isLocked = false;
        PlayerPrefs.SetInt(ch.name, 0);
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);

        UpdateUI();
    }

    public void UpdateUI()
    {
        ShopElement ch = characters[currentCharacterIndex];
        coinsText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();

        if (ch.isLocked)
        {
            playButton.gameObject.SetActive(false);

            // adButton.gameObject.SetActive(true);
            // adButton.interactable = true;

            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = ch.price + "";

            if (PlayerPrefs.GetInt("TotalCoins", 0) < ch.price) {
                buyButton.interactable = false;
                }
            else
                buyButton.interactable = true;
        }
        else
        {
            // adButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
            
    }
}
