using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace YandereNext.Tools
{
	public class GeneralFunctions : MonoBehaviour
	{
		public static void FixStreetCamera()
		{
			var currentCam = Camera.main;
			//currentCam.GetComponent<AmplifyMotionPostProcess>()?.enabled = false;
			var comps = currentCam.GetComponent<PostProcessingBehaviour>();
			if (comps != null)
			{
				comps.enabled = false;
			}
		}
		public static void UnlockMech()
		{
			var chain = FindObjectOfType<ChainScript>();
			var tarp = chain.Tarp;
			Destroy(chain.gameObject);
			tarp.Prompt.enabled = true;
			tarp.enabled = true;
			tarp.Unwrap = true;
			tarp.Prompt.Hide();
			tarp.Prompt.enabled = false;
			tarp.Mecha.enabled = true;
			tarp.Mecha.Prompt.enabled = true;

			YandereScript y = FindObjectOfType<YandereScript>();
			y.transform.position = chain.transform.position;
			

		}
		public static void SpawnOsana()
		{
			StudentManagerScript manager = FindObjectOfType<StudentManagerScript>();
			manager.SpawnStudent(10);
			manager.SpawnStudent(11);
			manager.Students[10].transform.position = new Vector3(0, -75, 0);
			manager.Students[11].transform.position = new Vector3(0, -75, 0);
		}
		public static GameObject GetStudent(int id)
		{
			StudentManagerScript manager = GameObject.FindObjectOfType<StudentManagerScript>();
			//var temp = manager.JSON;
			if (manager.Students == null || manager.Students[id].Spawned == false)
				return null;
			
			return manager.Students[id].gameObject;
		}

		public static void UpdateStudentHair(int student,string hair)
		{
			StudentManagerScript manager = GameObject.FindObjectOfType<StudentManagerScript>();
			if (manager.Students?[student]?.Hairstyle != null) {
				StudentScript s = manager.Students[student];
				s.Hairstyle = hair;
			}
			else
				Debug.Log("Student not found to change hair, the student maybe didn't spawn yet");
		}
		public static void SetPersona(int id)
		{
			YandereScript script = UnityEngine.Object.FindObjectOfType<YandereScript>();
			if (script != null)
			{
				script.UpdatePersona(id);
			}
		}
		public static void SetHairstyle(int id)
		{
			YandereScript script = UnityEngine.Object.FindObjectOfType<YandereScript>();
			if (script != null)
			{
				script.Hairstyle = id;
				script.UpdateHair();
			}
		}
		public static void PrintInfo()
		{
			YandereScript yandere = GameObject.FindObjectOfType<YandereScript>();
		}
		public static void SetAccessory(int id)
		{
			YandereScript script = GameObject.FindObjectOfType<YandereScript>();
			if (script != null)
			{
				script.AccessoryID = id;
				script.UpdateAccessory();
			}
		}
		public static Type GetTypeFromString(string type)
		{
			Assembly asm = typeof(GameObject).Assembly;
			Assembly asm1 = typeof(YandereScript).Assembly;
			Type t = asm.GetType("UnityEngine." + type) ?? asm1.GetType(type);
			if (t == null)
			{
				Debug.Log("There is an error in the name of the type or this type isn't a valid one");
				return null;
			}
			return t;
		}

		public static Type GetType(object thing)
		{
			return thing.GetType();
		}

		public static UnityEngine.Object FindObjectOfType(string type)
		{
			return UnityEngine.Object.FindObjectOfType(GetTypeFromString(type));
		}

		public static GameObject[] FindHiddenObjects(string name)
		{
			var objects = Resources.FindObjectsOfTypeAll<GameObject>();
			var lo = new List<GameObject>();
			foreach (GameObject o in objects)
			{
				if (o.name == name)
					lo.Add(o);
			}
			return lo.ToArray();
		}

		public static bool Inherits<T>(Type t)
		{
			return t.IsSubclassOf(typeof(T));
		}

		public static bool Inherits(Type t1, Type t2)
		{
			return t1.IsSubclassOf(t2);
		}

	}
}
