﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public class V2GameSeat : MMaterial
	{
		[SerializeField] private string objectName;
		[SerializeField] private MeshRenderer localScreen;

		public override void UpdateMaterial()
		{
			if (meshRenderers == null || meshRenderers.Length == 0)
				Start();

			base.UpdateMaterial();
		}

		private void Start()
		{
			var g = GameObject.Find(objectName);
			meshRenderers = new MeshRenderer[2];
			meshRenderers[0] = g.GetComponent<MeshRenderer>();
			meshRenderers[1] = localScreen;

			UpdateMaterial();
		}
	}
}