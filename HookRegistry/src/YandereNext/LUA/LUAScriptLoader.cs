using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace YandereNext.LUA
{
	public class LUAScriptLoader : ScriptLoaderBase
	{
		public override bool ScriptFileExists(string path)
		{
		    string ext = Path.GetExtension(path);
			bool exists = File.Exists(path) && !string.IsNullOrEmpty(ext) && (ext == ".lua" || ext == ".LUA");
			return exists;
		}

		public override object LoadFile(string file, Table globalContext)
		{
			return File.ReadAllText(file);
		}
	}
}
