﻿using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTarget : MEventSender
	{
		[Header("_" + nameof(MTarget))]
		[SerializeField] private MTargetUI[] mTargetUIs;

		[SerializeField] private bool useNone = true;
		[SerializeField] private string autoTargetName = "-";
		
		// If You Use UdonGraph, Don't Change The '_curTargetPlayerID' Variable's Access Modifier To Private.
		// If You Don't, You Can Just Change It If You Want.
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(CurTargetPlayerID))]
		private int _curTargetPlayerID = NONE_INT;
		public int CurTargetPlayerID
		{
			get => _curTargetPlayerID;
			set
			{
				_curTargetPlayerID = value;

				foreach (MTargetUI mTargetUI in mTargetUIs)
					mTargetUI.UpdateTargetPlayerUI(_curTargetPlayerID);

				SendEvents();
			}
		}

		public int[] PlayerIDBuffer { get; private set; } = new int[80];

		public bool IsLocalPlayerTarget => IsLocalPlayerID(CurTargetPlayerID);

		public VRCPlayerApi GetTargetPlayerAPI() => VRCPlayerApi.GetPlayerById(CurTargetPlayerID);

		public void SelectPlayer(int index)
		{
			SetOwner();
			CurTargetPlayerID = PlayerIDBuffer[index - 1];
			RequestSerialization();
		}

		public void SetPlayer(int id)
		{
			SetOwner();
			CurTargetPlayerID = id;
			RequestSerialization();
		}

		public void SetLocalPlayer()
		{
			SetOwner();
			CurTargetPlayerID = Networking.LocalPlayer.playerId;
			RequestSerialization();
		}

		public void SetNone()
		{
			SetOwner();
			CurTargetPlayerID = NONE_INT;
			RequestSerialization();
		}

		public void UpdateUI()
		{
			foreach (var mTargetUI in mTargetUIs)
				mTargetUI.UpdatePlayerList();
		}

		// ---- ---- ---- ----

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			foreach (MTargetUI mTargetUI in mTargetUIs)
			{
				mTargetUI.SetMTarget(this);
				mTargetUI.SetNoneButton(useNone);
			}

			if (Networking.LocalPlayer.isMaster)
			{
				SetOwner();
				CurTargetPlayerID = useNone ? NONE_INT : Networking.LocalPlayer.playerId;
				RequestSerialization();
			}
		}

		public override void OnPlayerJoined(VRCPlayerApi player)
		{
			if (IsOwner() && (Networking.LocalPlayer.displayName == autoTargetName))
			{
				CurTargetPlayerID = Networking.LocalPlayer.playerId;
				RequestSerialization();
			}
			else
			{
				UpdateUI();
			}
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (IsOwner() && (player.playerId == CurTargetPlayerID))
			{
				CurTargetPlayerID = useNone ? NONE_INT : Networking.LocalPlayer.playerId;
				RequestSerialization();
			}
			else
			{
				UpdateUI();
			}
		}
	}
}