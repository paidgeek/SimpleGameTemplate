using UnityEngine;

[CreateAssetMenu]
public class ShopProduct : ScriptableObject
{
	public enum Category
	{
		Permanent,
		SingleUse
	}

	[SerializeField]
	private long m_Id;
	[SerializeField]
	private Category m_Category;
	[SerializeField]
	private Sprite m_Icon;
	[SerializeField]
	private string m_Label;
	[SerializeField]
	private int m_Price;

	public long id
	{
		get { return m_Id; }
	}

	public Category category
	{
		get { return m_Category; }
	}

	public string label
	{
		get { return m_Label; }
	}

	public int price
	{
		get { return m_Price; }
	}

	public Sprite icon
	{
		get { return m_Icon; }
	}
}