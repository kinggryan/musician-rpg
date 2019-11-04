using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NotificationBoard {

	public delegate void NotificationFunc(object sender, object arg);

	private static Dictionary<string, List<NotificationFunc>> registeredListeners = new Dictionary<string, List<NotificationFunc>>();

	static public void AddListener(string notificationName, NotificationFunc func) {
		if(registeredListeners.ContainsKey(notificationName)) {
			var listenerMethods = registeredListeners[notificationName];
			listenerMethods.Add(func);
			registeredListeners[notificationName] = listenerMethods;
		} else {
			registeredListeners[notificationName] = new List<NotificationFunc>(new NotificationFunc[]{func});
		}
	}

	static public void RemoveListener(string notificationName, NotificationFunc func) {
		if(!registeredListeners.ContainsKey(notificationName))
			return;

		var listeners = registeredListeners[notificationName];
		listeners.Remove(func);
		registeredListeners[notificationName] = listeners;
	}
	
	static public void SendNotification(string notificationName, object sender, object arg) {
		if(!registeredListeners.ContainsKey(notificationName))
			return;

		var listeners = registeredListeners[notificationName];
		foreach(var listener in listeners) {
			listener(sender, arg);
		}
	}
}
