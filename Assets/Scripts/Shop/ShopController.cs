using System;
using UnityEngine;

public class ShopController : Singleton<ShopController>
{
  [SerializeField]
  private DataBindContext m_DataBindContext;

  private void Start()
  {
    var products = ShopInventory.instance.products;

    for (var categoryIndex = 0;
      categoryIndex < Enum.GetValues(typeof(ShopProduct.Category))
        .Length;
      categoryIndex++) {
      var category = (ShopProduct.Category) categoryIndex;
      var list = new ObservableList(category.ToString());

      for (var i = 0; i < products.Length; i++) {
        if (products[i].category == category) {
          list.Add(products[i]);
        }
      }

      m_DataBindContext[category.ToString()] = list;
    }
  }

  private new void OnEnable()
  {
    base.OnEnable();

    m_DataBindContext.BindAll();
  }

  public void Buy(ShopProduct product)
  {
    ShopInventory.instance.Buy(product);

    m_DataBindContext["coins"] = GameData.instance.coins;
  }
}