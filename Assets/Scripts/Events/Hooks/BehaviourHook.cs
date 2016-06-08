using UnityEngine;
using System.Collections;

public abstract class BehaviourHook : MonoBehaviour, IEventHook
{
	[SerializeField]
	protected string m_EventId;

	public void OnInvoke(string eventId)
	{
		if (m_EventId == eventId) {
			OnInvoke();
		}
	}

	protected virtual void OnInvoke() {}
}
