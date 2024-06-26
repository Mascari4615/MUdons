﻿using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MTargetBool : CustomBool
	{
		[field: Header("_" + nameof(MTargetBool))]
		[field: SerializeField] public MTarget MTarget { get; private set; }

		protected override void Start()
		{
			OnValueChange();
		}

		// Should Called By MTarget Change Event
		public void UpdateValue()
		{
			SetValue(MTarget.IsTargetPlayer());
		}
	}
}