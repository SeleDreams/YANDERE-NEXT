using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MoonSharp.Interpreter;

namespace YandereNext.LUA.Skeletons
{
	class MonoBehaviourSkeleton
	{

		public MonoBehaviourSkeleton(MonoBehaviour script)
		{
			_script = script;
			trueType = _script.GetType();
		}
		
		
		readonly Type trueType;
		public MonoBehaviour _script;
	}
}
