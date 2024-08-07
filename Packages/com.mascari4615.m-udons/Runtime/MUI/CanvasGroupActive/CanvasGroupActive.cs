﻿using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasGroupActive : MEventSender
	{
		[Header("_" + nameof(ObjectActive))]
		[SerializeField] private CanvasGroup[] activeCanvasGroups;
		[SerializeField] private CanvasGroup[] disableCanvasGroups;
		[SerializeField] private Image[] buttonUIImages;
		[SerializeField] private Sprite[] buttonUISprites;
		[SerializeField] private bool defaultActive;
		[SerializeField] private CustomBool customBool;

		private bool active;
		public bool Active
		{
			get => active;
			private set
			{
				active = value;
				OnActiveChange();
			}
		}

		private void Start()
		{
			if (customBool == null)
			{
				Active = defaultActive;
			}
			else
			{

			}

			OnActiveChange();
		}

		public void SetActive(bool targetActive)
		{
			MDebugLog($"{nameof(SetActive)}({targetActive})");

			Active = targetActive;
		}

		[ContextMenu(nameof(ToggleActive))]
		public void ToggleActive() => SetActive(!Active);

		[ContextMenu(nameof(SetActiveTrue))]
		public void SetActiveTrue() => SetActive(true);

		[ContextMenu(nameof(SetActiveFalse))]
		public void SetActiveFalse() => SetActive(false);

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			if (customBool)
				SetActive(customBool.Value);
		}

		public void SetCustomBool(CustomBool customBool)
		{
			this.customBool = customBool;
			UpdateValue();
		}

		private void OnActiveChange()
		{
			MDebugLog($"{nameof(OnActiveChange)}");

			foreach (Image i in buttonUIImages)
			{
				if (buttonUISprites != null && buttonUISprites.Length > 0)
					i.sprite = buttonUISprites[Active ? 0 : 1];
				else
					i.color = MColorUtil.GetGreenOrRed(Active);
			}

			foreach (CanvasGroup c in activeCanvasGroups)
				SetCanvasGroup(c, Active);

			foreach (CanvasGroup c in disableCanvasGroups)
				SetCanvasGroup(c, !Active);


			SendEvents();
		}

		private void SetCanvasGroup(CanvasGroup canvasGroup, bool enabled)
		{
			canvasGroup.alpha = enabled ? 1 : 0;
			canvasGroup.blocksRaycasts = enabled;
			canvasGroup.interactable = enabled;
		}
	}
}