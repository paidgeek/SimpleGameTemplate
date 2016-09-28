using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : Singleton<ShopInventory>
{
	public ShopProduct[] products { get; private set; }
	public Dictionary<long, int> inventory { get; private set; }

	private void Awake()
	{
		products = Resources.LoadAll<ShopProduct>("Data/Products");
		inventory = new Dictionary<long, int>();

		for (var i = 0; i < products.Length; i++) {
			var product = products[i];
			var amount = PlayerPrefs.GetInt("Amount" + product.id, 0);

			inventory[product.id] = amount;
		}
	}

	public bool CanBuy(ShopProduct product)
	{
		return GameData.instance.coins >= product.price;
	}

	public void Buy(ShopProduct product)
	{
		if (!CanBuy(product)) {
			return;
		}

		GameData.instance.coins -= product.price;
		inventory[product.id]++;

		Save();
	}

	private void Save()
	{
		for (var i = 0; i < products.Length; i++) {
			var product = products[i];

			PlayerPrefs.SetInt("Amount" + product.id, inventory[product.id]);
		}

		PlayerPrefs.Save();
	}
}