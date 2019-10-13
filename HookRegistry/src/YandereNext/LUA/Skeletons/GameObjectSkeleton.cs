using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MoonSharp.Interpreter;

namespace YandereNext.LUA.Skeletons
{
	[MoonSharpUserData]
	public class GameObjectSkeleton
	{

		private GameObject _go;
		
		public GameObjectSkeleton()
		{
			_go = null;
		}
		
		public static GameObjectSkeleton CreateInstance()
		{
			return new GameObjectSkeleton();
		}

		public string name
		{
			get => GetName();
			set => SetName(value);
		}

		private string GetName()
		{
			if (_go != null)
			{
				return _go.name;
			}
			else
			{
				Debug.Log("No gameobject currently selected");
				return string.Empty;
			}
		}

		private void SetName(string name)
		{
			if (_go != null)
			{
				_go.name = name;
			}
			else
			{
				Debug.Log("No GameObject currently selected");
			}
		}

		public bool Find(string name)
		{
			Debug.Log("GameObject.Find called");
			_go = GameObject.Find(name);

			if (_go != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
