﻿using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class SendEventOnPickupUse : MEventSender
	{
		public override void OnPickupUseDown()
		{
			SendEvents();
		}
	}
}