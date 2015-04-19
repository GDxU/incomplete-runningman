using UnityEditor;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles ()
	{
		BuildPipeline.BuildAssetBundles ("AssetBundles");

		// Put the bundles in a folder called "ABs" within the
		// Assets folder.
		// BuildPipeline.BuildAssetBundles("Assets/ABs");
	}

	

}