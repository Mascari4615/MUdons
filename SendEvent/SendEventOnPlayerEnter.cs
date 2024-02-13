﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnPlayerEnter : MEventSender
	{
		[Header("_" + nameof(SendEventOnPlayerExit))]
		[SerializeField] private bool onlyIfLocalPlayer = true;

		public override void OnPlayerTriggerEnter(VRCPlayerApi player)
		{
			if (onlyIfLocalPlayer && (player != Networking.LocalPlayer))
				return;

			SendEvents();
		}
	}
}