using System;
using System.Collections;
using UnityEngine;
using YandereNext.Tools;
namespace YandereNext.LUA.Skeletons
{
	public class RetargetBone : MonoBehaviour
	{
		public Transform target;
		public bool root = false;
		Vector3 BaseRotation = new Vector3();
		//Vector3 BaseGlobalRotation = new Vector3();
		float BasePositionY = 0f;
		float OtherBasePositionY = 0f;

		IEnumerator RetargetAll(Transform ts)
		{
			foreach (Transform t in ts)
			{
				Transform ctarget = target.Find(t.name);
				if (ctarget != null)
				{
					t.gameObject.AddComponent<RetargetBone>().target = ctarget;
				}
			}
			yield return null;
		}

		void Start()
		{
			
			BaseRotation = transform.localEulerAngles;
			BasePositionY = transform.localPosition.y;
			OtherBasePositionY = target.transform.localPosition.y;
			if (root)
			{
				transform.parent = target.parent;
			}

			if (transform.childCount > 0)
			{
				StartCoroutine(RetargetAll(transform));
			}
		}


		void LateUpdate()
		{
			
			if (target != null)
			{
				if (root)
				{

					transform.position = target.transform.position;
					transform.rotation = target.transform.rotation;
					//transform.localPosition = Vector3.zero;
					//transform.localScale = target.transform.localScale;
				}
				else
				{
					transform.localPosition = target.transform.localPosition;
					//Debug.Log(transform.name + " is retargeting " + target.name);
					transform.localEulerAngles = target.localEulerAngles;
					transform.rotation = target.rotation;
					//Debug.Log(name + " " + transform.eulerAngles);
				}
			}
		}
	}
	// Token: 0x02000004 RID: 4
	public class ModelRetarget
	{
		public static void UpdateRenderers(GameObject current)
		{
			var selectedMesh = current.GetComponentsInChildren<SkinnedMeshRenderer>() ?? current.GetComponents<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer a in selectedMesh)
			{
				a.updateWhenOffscreen = true;
			}
		}

		public static void DisableRenderers(GameObject target,bool fullDisable = false)
		{
			var targetMesh = target.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer a in targetMesh)
			{
				a.sharedMesh = new Mesh();
				if (fullDisable)
				{
					a.enabled = false;
				}
			}
		}

		public static void DisableSimilarRenderers(GameObject a,GameObject b)
		{
			var sourceStaticMesh = a.GetComponentsInChildren<MeshRenderer>() ?? a.GetComponents<MeshRenderer>();
			var targetStaticMesh = b.GetComponentsInChildren<MeshRenderer>() ?? b.GetComponents<MeshRenderer>();
			foreach (MeshRenderer source in sourceStaticMesh)
			{
				foreach (MeshRenderer target in targetStaticMesh)
				{
					if (target.name == source.name)
					{
						target.enabled = false;
					}
				}
			}
		}
		public static void DisableScripts(GameObject target)
		{
			var selectedScripts = target.GetComponentsInChildren<MonoBehaviour>() ?? target.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour a in selectedScripts)
			{
				a.enabled = false;
			}
		}

		public static void DisableAnims(GameObject source)
		{
			var selectedAnims = source.GetComponentsInChildren<Animation>() ?? source.GetComponents<Animation>();
			foreach (Animation a in selectedAnims)
			{
				a.enabled = false;
			}
		}
		public static bool RetargetModel(GameObject trueSelected, GameObject target,bool fullDisable = false)
		{
			if (trueSelected == null || target == null)
			{
				Debug.Log("The selected gameobject or the target gameobject is null");
				return false;
			}

			GameObject selected = trueSelected;
			var childrenSelected = selected.GetComponentsInChildren<Transform>();
			var childrenTarget = target.GetComponentsInChildren<Transform>();
			selected.transform.localPosition = target.transform.localPosition;
			selected.transform.localRotation = target.transform.localRotation;
			selected.layer = target.layer;
			UpdateRenderers(selected);
			DisableScripts(selected);
			DisableRenderers(target,fullDisable);
			DisableSimilarRenderers(selected, target);
			DisableAnims(selected);
			var selectedRetarget = selected.AddComponent<RetargetBone>();
			selectedRetarget.root = true;
			selectedRetarget.target = target.transform;
			return true;
		}

	}
}
