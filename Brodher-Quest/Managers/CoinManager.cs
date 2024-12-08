using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : SingletonBehaviour<CoinManager>
{
    //
    //  Editor settings
    //


    [SerializeField] private TMP_Text text;
    
    
    //
    //  Class variables
    //


    private Coin[] coinObjects;

    string data;
    private int collectedCoins;
    public int coins { get; private set; }

    private AudioSource source;


    //  
    //  Unity Events
    //


    void Start()
    {
        source = GetComponent<AudioSource>();

        coinObjects = GetComponentsInChildren<Coin>();

        data = PlayerPrefs.GetString($"{SceneManager.GetActiveScene().name}-coins", GetNewData());
        coins = PlayerPrefs.GetInt("coins", 0);

        for(int i = 0; i < data.Length; i++)
		{
            coinObjects[i].listIndex = i;
            if (data[i] == 'N')
                Destroy(coinObjects[i].gameObject);
        }

        //text.text = $"x{coins + collectedCoins}";
    }


    //
    //  Methods
    //


    public void Save()
    {
        PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name}-coins", data);
        PlayerPrefs.SetInt("coins", collectedCoins + PlayerPrefs.GetInt("coins", 0));
    }


    public void CoinCollected(Coin coin)
	{
        source.Play();

		collectedCoins++;
        //text.text = $"x{coins + collectedCoins}";

        data = data.Remove(coin.listIndex, 1).Insert(coin.listIndex, "N");

        Save();
    }


	public string GetNewData()
	{
        string str = "";

        for (int i = 0; i < coinObjects.Length; i++)
            str += "Y";

        return str;
	}
}
