﻿using UnityEngine;

namespace Mascari4615
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectActive : ActiveToggle
	{
		[Header("_" + nameof(ObjectActive))]
		[SerializeField] private GameObject[] activeObjects;
		[SerializeField] private GameObject[] disableObjects;

		protected override void UpdateActive()
		{
			MDebugLog($"{nameof(UpdateActive)}");

			foreach (GameObject o in activeObjects)
				o.SetActive(Active);

			foreach (GameObject o in disableObjects)
				o.SetActive(!Active);
		}
	}
}