﻿using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class TGameBank : MBase
	{
		public readonly int[] MAX_COIN_BY_ROUND = new int[] { 50, 100, 100, 100, 100, 100 };

		[Header("_" + nameof(TGameBank))]
		[SerializeField] private TGameManager gameManager;

		[SerializeField] private MScore stealCoinAmount;
		[SerializeField] private MScoreSlider mScoreSlider;
		[SerializeField] private TextMeshProUGUI remainCoinText;
		[SerializeField] private GameObject closedUI;

		[SerializeField] private MeshRenderer[] goldBars;
		[SerializeField] private Material originGoldBarMaterial;
		[SerializeField] private Material selectedGoldBarMaterial;

		[SerializeField] private SyncedBool hasGateOpen;

		public int RemainCoin
		{
			get => _bankCoin;
			set
			{
				_bankCoin = value;
				UpdateUI();
				mScoreSlider.Init();
			}
		}
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(RemainCoin))] private int _bankCoin = 0;

		private void Start()
		{
			UpdateUI();
		}

		public void UpdateUI()
		{
			remainCoinText.text = _bankCoin.ToString();

			if (gameManager.IsGaming == false)
				return;

			stealCoinAmount.SetMinMaxScore(min: 0, max: Mathf.Min(RemainCoin, MAX_COIN_BY_ROUND[gameManager.Data.CurRound] / 5));
			closedUI.SetActive((int)gameManager.CurRoundData.CurState > (int)TGameRoundState.Steal6);

			for (int i = goldBars.Length - 1; i >= 0; i--)
				goldBars[i].gameObject.SetActive(i + 1 > goldBars.Length - RemainCoin);

			// [Front/0] ~ (StealCoin) ~ (RemainCoin) ~ [Back/100]

			for (int i = goldBars.Length - 1; i >= 0; i--)
				goldBars[i].material = (
					i >= (goldBars.Length - RemainCoin) &&
					i < (goldBars.Length - RemainCoin) + stealCoinAmount.Score)
					? selectedGoldBarMaterial
					: originGoldBarMaterial;
		}

		public void GetCoin()
		{
			if (!gameManager.IsGaming)
				return;

			if ((int)gameManager.CurRoundData.CurState > (int)TGameRoundState.Steal6)
				return;

			int curPlayerIndex = gameManager.CurRoundData.NumberByOrder[(int)gameManager.CurRoundData.CurState - (int)TGameRoundState.Steal0];
			if (!gameManager.IsLocalPlayerNumber(curPlayerIndex))
				return;

			int stealAmount = stealCoinAmount.Score;
			if (RemainCoin < stealCoinAmount.Score)
				return;

			SetOwner();
			RemainCoin -= stealAmount;

			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
			{
				if (gameManager.CurRoundData.CoinByOrder[i] == NONE_INT)
				{
					gameManager.CurRoundData.CoinByOrder[i] = stealAmount;
					break;
				}
			}

			gameManager.Data.SyncGameData();
			RequestSerialization();

			stealCoinAmount.SetScore(0);
			mScoreSlider.Init();
			hasGateOpen.SetValue(true);
		}

		public void InitCoin()
		{
			SetOwner();
			RemainCoin = MAX_COIN_BY_ROUND[gameManager.Data.CurRound];
			RequestSerialization();
		}
	}
}