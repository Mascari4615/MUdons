﻿using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class UIMString : UdonSharpBehaviour
	{
		[Header("_" + nameof(UIMString))]
		[SerializeField] private MString mString;
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private TextMeshProUGUI[] texts;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			mString.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			string newText = mString.GetFormatString();

			inputField.text = newText;

			foreach (TextMeshProUGUI child in texts)
				child.text = newText;
		}

		public void SubmitInputField()
		{
			string newText = inputField.text;
			newText = newText.TrimStart('\n', ' ');
			newText = newText.TrimEnd('\n', ' ');

			bool IsVaildText = mString.IsVaildText(newText);

			if (IsVaildText)
				mString.SetValue(newText);
			else
			{
				// TODO: 직접 InputField에 표시하지 않고 (기존 입력이 날라가니까) 다른 방법으로
				inputField.text = "유효한 문자열이 아닙니다.";
			}
		}
	}
}