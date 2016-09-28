using UnityEngine;
using UnityEngine.UI;

public class ShopProductView : MonoBehaviour, IModel
{
	[SerializeField]
	private Button m_BuyButton;

	public ShopProduct product
	{
		get { return (ShopProduct) model; }
	}

	public object model { get; set; }

	private void Start()
	{
		m_BuyButton.interactable = ShopInventory.instance.CanBuy(product);
	}

	public void OnBuyClick()
	{
		ShopController.instance.Buy(product);
		m_BuyButton.interactable = ShopInventory.instance.CanBuy(product);
	}
}