using Godot;
using Godot.Collections;
using System;

public partial class Saves : Node
{
	public static bool WantsFullScreen = false;
	public static bool WantsScreenShake = true;
	public static bool HasEndless = false;

	public static void SaveGame() {
		using var saveGame = FileAccess.Open("user://SaveData.save", FileAccess.ModeFlags.Write);
		var data = new Dictionary {
			{"FullScreen", WantsFullScreen},
			{"Screen Shake", WantsScreenShake},
			{"Endless Unlocked", HasEndless}
		};

		var json = new Json();
		string jsonString = Json.Stringify(data);

		saveGame.StoreString(jsonString);
	}

	public static void LoadGame() {
    	if (!FileAccess.FileExists("user://SaveData.save")) {
     	   return;
    	}

		using var saveGame = FileAccess.Open("user://SaveData.save", FileAccess.ModeFlags.Read);
		string jsonContent = saveGame.GetAsText();

		var json = new Json();
		var err = json.Parse(jsonContent);
		if (err == Error.Ok) {
			Dictionary recieved = (Dictionary)json.Data;
			
			WantsFullScreen = (bool)recieved["FullScreen"];
			WantsScreenShake = (bool)recieved["Screen Shake"];
			HasEndless = (bool)recieved["Endless Unlocked"];

		} else {
			GD.Print("JSON Parse Error: ", json.GetErrorMessage(), " in ", jsonContent, " at line ", json.GetErrorLine());
		}
	}
}
