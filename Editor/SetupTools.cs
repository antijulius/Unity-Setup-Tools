using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;

namespace aj
{
	public class ToolsMenu : MonoBehaviour
	{
		[MenuItem("Tools/Setup/Create Default Folders")]
		public static void CreateDefaultFolders()
		{
			CreateDirectories("_Project", "Scripts", "Art", "Scenes", "Ignore");
			CreateDirectories(Combine("_Project", "Tests"), "EditMode", "PlayMode");
			Refresh();
		}

		[MenuItem("Tools/Setup/Fetch .gitignore")]
		public static async void FetchGitignore()
		{
			var url = GetGistUrl("06b39890e0b9e9676fcb8f6265424fa9");
			var content = await GetContent(url);
			await File.WriteAllTextAsync(Combine(dataPath, "../.gitignore"), content);
		}

		[MenuItem("Tools/Packages/Get New Input System")]
		static void AddNewInputSystem() => InstallPackage("inputsystem");
		
		[MenuItem("Tools/Packages/Get Cinemachine")]
		static void AddCinemachine() => InstallPackage("cinemachine");
		
		[MenuItem("Tools/Packages/Get Shader Graph")]
		static void AddShaderGraph() => InstallPackage("shadergraph");
		
		[MenuItem("Tools/Packages/Get Terrain Tools")]
		static void AddTerrainTools() => InstallPackage("terrain-tools");
		
		[MenuItem("Tools/Packages/Get ProBuilder")]
		static void AddProBuilder() => InstallPackage("probuilder");

		[MenuItem("Tools/Packages/Get All")]
		static void GetAllPackages()
		{
			AddNewInputSystem();
			AddCinemachine();
			AddShaderGraph();
			AddTerrainTools();
			AddProBuilder();
		}

		private static void CreateDirectories(string root, params string[] dirs)
		{
			var fullPath = Combine(dataPath, root);

			foreach (var newDir in dirs)
			{
				CreateDirectory(Combine(fullPath, newDir));
			}
		}

		static string GetGistUrl(string id, string user = "antijulius") =>
			$"https://gist.githubusercontent.com/{user}/{id}/raw/";

		static async Task<string> GetContent(string url)
		{
			using var client = new HttpClient();
			var response = await client.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();
			return content;
		}

		static void InstallPackage(string packageName)
		{
			UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
		}
	}
}