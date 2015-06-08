using System;
using System.IO;

namespace BCHAFormulary
{
	public class FileAccessHelper
	{
		string fullFileName;

		public FileAccessHelper (Environment.SpecialFolder filePath, string fileName)
		{
			var path = Environment.GetFolderPath (filePath);
			fullFileName = Path.Combine (path, fileName);
		}

		public bool saveFile(string saveContent){
			bool success = false;
			try{
				using (var streamWriter = new StreamWriter(fullFileName,false))
				{
					streamWriter.WriteLine(saveContent);
					success = true;
				}
			}
			catch(Exception e){
				Console.WriteLine ("Could not save to {0} due to {1}", fullFileName, e.Message);
			}
			return success;
		}

		public string loadFile(){
			try{
				using (var streamReader = new StreamReader(fullFileName))
				{
					return streamReader.ReadToEnd();
				}
			}
				catch(Exception e){
					Console.WriteLine ("Could not load file {0} due to {1}", fullFileName, e.Message);
					return string.Empty;
				}
		}
		}
}

