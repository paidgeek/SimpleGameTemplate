using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
	public void Invoke(string eventId)
	{
		var hooks = GetComponentsInChildren<IEventHook>(true);

		for (var i = 0; i < hooks.Length; i++) {
			hooks[i].OnInvoke(eventId);
		}
	}
}