﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


[System.Serializable]
public class Item
{

	public string itemName;
	public Sprite icon;
	public float price = 1;


}

public class ShopScrollList : MonoBehaviour {
	public Text ItemText;
	public Text LabelText;
	public Image Bigimage;
	public List<Item> itemList;
	public Transform contentPanel;
	public ShopScrollList otherShop;
	public Text myGoldDisplay;
	public SimpleObjectPool buttonObjectPool;


	public float gold = 20f;


	// Use this for initialization
	void Start () 
	{
		LabelText.enabled = false;
		Bigimage.enabled = false;
		RefreshDisplay ();
	}

	public void RefreshDisplay()
	{
		
		myGoldDisplay.text = "Gold: " + gold.ToString ();
		RemoveButtons ();
		AddButtons ();
	}

	private void RemoveButtons()
	{
		while (contentPanel.childCount > 0) 
		{
			GameObject toRemove = transform.GetChild(0).gameObject;
			buttonObjectPool.ReturnObject(toRemove);
		}
	}

	private void AddButtons()
	{
		for (int i = 0; i < itemList.Count; i++) 
		{
			Item item = itemList[i];
			GameObject newButton = buttonObjectPool.GetObject();
			newButton.transform.SetParent(contentPanel,false);

			SampleButton sampleButton = newButton.GetComponent<SampleButton>();
			sampleButton.Setup(item, this);
		}
	}

	public void TryTransferItemToOtherShop(Item item)
	{
		LabelText.enabled = true;
		Bigimage.enabled = true;
		Bigimage.sprite = item.icon;
		ItemText.text = item.itemName;

		if (otherShop.gold >= item.price) 
		{
			gold += item.price;
			otherShop.gold -= item.price;

			AddItem(item, otherShop);
			RemoveItem(item, this);

			RefreshDisplay();
			otherShop.RefreshDisplay();
			Debug.Log ("enough gold");

		}
		Debug.Log ("attempted");
	}

	void AddItem(Item itemToAdd, ShopScrollList shopList)
	{
		shopList.itemList.Add (itemToAdd);
	}

	private void RemoveItem(Item itemToRemove, ShopScrollList shopList)
	{
		for (int i = shopList.itemList.Count - 1; i >= 0; i--) 
		{
			if (shopList.itemList[i] == itemToRemove)
			{
				shopList.itemList.RemoveAt(i);
			}
		}
	}
}