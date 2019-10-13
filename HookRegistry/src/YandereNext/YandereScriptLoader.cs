using System;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;
namespace YandereNext
{
	public class YandereScriptLoader : ScriptLoaderBase
	{
		Script _luaScript;
		string _scriptFile;
		string name;

		public override bool ScriptFileExists(string path)
		{
			return ScriptExists(path);
		}

		public override object LoadFile(string file, Table globalContext)
		{
			return LoadScript(file);
		}


		bool ScriptExists(string path)
		{
			Debug.Log("Exists check " + path);
			string ext = Path.GetExtension(path);

			bool exists =
				File.Exists(path) &&
				!string.IsNullOrEmpty(ext) &&
				(ext == ".lua" || ext == ".LUA");

			return exists;
		}

		string LoadScript(string path)
		{
			Debug.Log("Loading " + path);
			return File.ReadAllText(path);

		}

	}
}
