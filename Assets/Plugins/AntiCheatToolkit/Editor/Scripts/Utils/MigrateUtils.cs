﻿#if UNITY_EDITOR

namespace CodeStage.AntiCheat.EditorCode
{
	using Common;
	using ObscuredTypes;

	using System;
	using System.Runtime.InteropServices;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Class with utility functions to help with ACTk migrations after updates.
	/// </summary>
	public class MigrateUtils
	{
		private enum MigrationSelection
		{
			Basic = 5,
			Unity = 10,
			All = 15
		}

		private static MigrationSelection currentSelection;

		/// <summary>
		/// Checks all prefabs in project for old version of obscured types and tries to migrate values to the new version.
		/// </summary>
		[MenuItem(ActEditorGlobalStuff.WindowsMenuPath + "Migrate obscured types on prefabs...", false, 1100)]
		public static void MigrateObscuredTypesOnPrefabs()
		{
			if (!EditorUtility.DisplayDialog("ACTk Obscured types migration",
				"Are you sure you wish to scan all prefabs in your project and automatically migrate values to the new format?\n" + GetWhatMigratesString(),
				"Yes", "No"))
			{
				Debug.Log(ActEditorGlobalStuff.LogPrefix + "Obscured types migration was canceled by user.");
				return;
			}

			SelectionDialog();

			AssetDatabase.SaveAssets();

			var touchedCount = 0;
			try
			{
				var assets = AssetDatabase.FindAssets("t:ScriptableObject t:Prefab");
				var count = assets.Length;
				for (var i = 0; i < count; i++)
				{
					if (EditorUtility.DisplayCancelableProgressBar("Looking through objects", "Object " + (i + 1) + " from " + count,
						i / (float)count))
					{
						Debug.Log(ActEditorGlobalStuff.LogPrefix + "Obscured types migration was canceled by user.");
						break;
					}

					var guid = assets[i];
					var path = AssetDatabase.GUIDToAssetPath(guid);

					var objects = AssetDatabase.LoadAllAssetsAtPath(path);
					foreach (var unityObject in objects)
					{
						if (unityObject == null) continue;
						if (unityObject.name == "Deprecated EditorExtensionImpl") continue;

						var so = new SerializedObject(unityObject);
						var modified = MigrateObject(so, unityObject.name + "\n" + path);

						if (modified)
						{
							touchedCount++;
							so.ApplyModifiedProperties();
							EditorUtility.SetDirty(unityObject);
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
			finally
			{
				AssetDatabase.SaveAssets();
				EditorUtility.ClearProgressBar();
			}

			if (touchedCount > 0)
				Debug.Log(ActEditorGlobalStuff.LogPrefix + "Migrated obscured types on " + touchedCount + " objects.");
			else
				Debug.Log(ActEditorGlobalStuff.LogPrefix + "No objects were found for obscured types migration.");
		}

		/// <summary>
		/// Checks all scenes in project for old version of obscured types and tries to migrate values to the new version.
		/// </summary>
		[MenuItem(ActEditorGlobalStuff.WindowsMenuPath + "Migrate obscured types in opened scene(s)...", false, 1101)]
		public static void MigrateObscuredTypesInScene()
		{
			if (!EditorUtility.DisplayDialog("ACTk Obscured types migration",
				"Are you sure you wish to scan all opened scenes and automatically migrate values to the new format?\n" + GetWhatMigratesString(),
				"Yes", "No"))
			{
				Debug.Log(ActEditorGlobalStuff.LogPrefix + "Obscured types migration was canceled by user.");
				return;
			}

			SelectionDialog();

			var touchedCount = 0;
			try
			{
				var allTransformsInOpenedScenes = Resources.FindObjectsOfTypeAll<Transform>();
				var count = allTransformsInOpenedScenes.Length;
				var updateStep = Math.Max(count / 10, 1);

				for (var i = 0; i < count; i++)
				{
					var transform = allTransformsInOpenedScenes[i];
					if (i % updateStep == 0 && EditorUtility.DisplayCancelableProgressBar("Looking through objects", "Object " + (i + 1) + " from " + count,
						    i / (float)count))
					{
						Debug.Log(ActEditorGlobalStuff.LogPrefix + "Obscured types migration was canceled by user.");
						break;
					}

					if (transform == null) continue;

					var components = transform.GetComponents<Component>();
					foreach (var component in components)
					{
						if (component == null) continue;

						var so = new SerializedObject(component);
						var modified = MigrateObject(so, transform.name);

						if (modified)
						{
#if UNITY_5_3_OR_NEWER
							UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
#else
							EditorApplication.MarkSceneDirty();
#endif
							touchedCount++;
							so.ApplyModifiedProperties();
							EditorUtility.SetDirty(component);
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
			finally
			{
				if (touchedCount > 0)
				{
					EditorUtility.DisplayDialog(touchedCount + " objects migrated", "Objects with old obscured types migrated: " + touchedCount + ".\nPlease save your scenes to keep the changes.", "Fine");

#if UNITY_5_3_OR_NEWER
					UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
#else
					EditorApplication.SaveCurrentSceneIfUserWantsTo();
#endif
				}

				AssetDatabase.SaveAssets();
				EditorUtility.ClearProgressBar();
			}

			if (touchedCount > 0)
				Debug.Log(ActEditorGlobalStuff.LogPrefix + "Migrated obscured types on " + touchedCount + " objects in opened scene(s).");
			else
				Debug.Log(ActEditorGlobalStuff.LogPrefix + "No objects were found in opened scene(s) for obscured types migration.");
		}

		private static void SelectionDialog()
		{
			var selection = EditorUtility.DisplayDialogComplex("Select migration targets",
				"WARNING: data can be damaged if you migrate already migrated Unity-specific types!\n" +
				"Press Basic to migrate basic types only (ObscuredFloat, ObscuredDouble).\n" +
				"Press Unity to migrate Unity-specific types only (ObscuredVector2/3, ObscuredQuaternion).\n" +
				"Press All to migrate all obscured types.",
				"Basic", "Unity", "All");

			currentSelection =
				selection == 0 ? MigrationSelection.Basic :
				selection == 1 ? MigrationSelection.Unity :
				MigrationSelection.All;
		}

		private static string GetWhatMigratesString()
		{
			return "ObscuredFloat, ObscuredDouble, ObscuredVector2, ObscuredVector3 and ObscuredQuaternion will be migrated.";
		}

		private static bool MigrateObject(SerializedObject so, string label)
		{
			var modified = false;

			var sp = so.GetIterator();
			if (sp == null) return false;

			while (sp.NextVisible(true))
			{
				if (sp.propertyType != SerializedPropertyType.Generic) continue;

				var type = sp.type;

				switch (type)
				{
					case "ObscuredDouble":
					{
						if (currentSelection == MigrationSelection.Unity) break;

						var hiddenValue = sp.FindPropertyRelative("hiddenValue");
						if (hiddenValue == null) continue;

						var hiddenValueOldProperty = sp.FindPropertyRelative("hiddenValueOldByte8");
						var hiddenValueOld = default(ACTkByte8);
						var oldValueExists = false;

						if (hiddenValueOldProperty != null)
						{
							if (hiddenValueOldProperty.FindPropertyRelative("b1") != null)
							{
								hiddenValueOld.b1 = (byte)hiddenValueOldProperty.FindPropertyRelative("b1").intValue;
								hiddenValueOld.b2 = (byte)hiddenValueOldProperty.FindPropertyRelative("b2").intValue;
								hiddenValueOld.b3 = (byte)hiddenValueOldProperty.FindPropertyRelative("b3").intValue;
								hiddenValueOld.b4 = (byte)hiddenValueOldProperty.FindPropertyRelative("b4").intValue;
								hiddenValueOld.b5 = (byte)hiddenValueOldProperty.FindPropertyRelative("b5").intValue;
								hiddenValueOld.b6 = (byte)hiddenValueOldProperty.FindPropertyRelative("b6").intValue;
								hiddenValueOld.b7 = (byte)hiddenValueOldProperty.FindPropertyRelative("b7").intValue;
								hiddenValueOld.b8 = (byte)hiddenValueOldProperty.FindPropertyRelative("b8").intValue;

								if (hiddenValueOld.b1 != 0 ||
								    hiddenValueOld.b2 != 0 ||
								    hiddenValueOld.b3 != 0 ||
								    hiddenValueOld.b4 != 0 ||
								    hiddenValueOld.b5 != 0 ||
								    hiddenValueOld.b6 != 0 ||
								    hiddenValueOld.b7 != 0 ||
								    hiddenValueOld.b8 != 0)
								{
									oldValueExists = true;
								}
							}
						}

						if (oldValueExists)
						{
							var union = new LongBytesUnion {b8 = hiddenValueOld};
							union.b8.Shuffle();
							hiddenValue.longValue = union.l;

							hiddenValueOldProperty.FindPropertyRelative("b1").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b2").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b3").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b4").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b5").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b6").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b7").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b8").intValue = 0;

							Debug.Log(ActEditorGlobalStuff.LogPrefix + "Migrated property " + sp.displayName + ":" + type +
							          " at the object " + label);
							modified = true;
						}

						break;
					}
					case "ObscuredFloat":
					{
						if (currentSelection == MigrationSelection.Unity) break;

						var hiddenValue = sp.FindPropertyRelative("hiddenValue");
						if (hiddenValue == null) continue;

						var hiddenValueOldProperty = sp.FindPropertyRelative("hiddenValueOldByte4");
						var hiddenValueOld = default(ACTkByte4);
						var oldValueExists = false;

						if (hiddenValueOldProperty != null)
						{
							if (hiddenValueOldProperty.FindPropertyRelative("b1") != null)
							{
								hiddenValueOld.b1 = (byte)hiddenValueOldProperty.FindPropertyRelative("b1").intValue;
								hiddenValueOld.b2 = (byte)hiddenValueOldProperty.FindPropertyRelative("b2").intValue;
								hiddenValueOld.b3 = (byte)hiddenValueOldProperty.FindPropertyRelative("b3").intValue;
								hiddenValueOld.b4 = (byte)hiddenValueOldProperty.FindPropertyRelative("b4").intValue;

								if (hiddenValueOld.b1 != 0 ||
								    hiddenValueOld.b2 != 0 ||
								    hiddenValueOld.b3 != 0 ||
								    hiddenValueOld.b4 != 0)
								{
									oldValueExists = true;
								}
							}
						}

						if (oldValueExists)
						{
							var union = new FloatIntBytesUnion { b4 = hiddenValueOld};
							union.b4.Shuffle();
							hiddenValue.longValue = union.i;

							hiddenValueOldProperty.FindPropertyRelative("b1").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b2").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b3").intValue = 0;
							hiddenValueOldProperty.FindPropertyRelative("b4").intValue = 0;

							Debug.Log(ActEditorGlobalStuff.LogPrefix + "Migrated property " + sp.displayName + ":" + type +
							          " at the object " + label);
							modified = true;
						}

						break;
					}
					case "ObscuredVector2":
					{
						if (currentSelection == MigrationSelection.Basic) break;		

						var hiddenValue = sp.FindPropertyRelative("hiddenValue");
						if (hiddenValue == null) continue;

						var hiddenValueX = hiddenValue.FindPropertyRelative("x");
						var hiddenValueY = hiddenValue.FindPropertyRelative("y");
						
						var union = new FloatIntBytesUnion { i = hiddenValueX.intValue };
						union.b4.Shuffle();
						hiddenValueX.intValue = union.i;

						union.i = hiddenValueY.intValue;
						union.b4.Shuffle();
						hiddenValueY.intValue = union.i;

						modified = true;

						break;
					}
					case "ObscuredVector3":
					{
						if (currentSelection == MigrationSelection.Basic) break;

						var hiddenValue = sp.FindPropertyRelative("hiddenValue");
						if (hiddenValue == null) continue;

						var hiddenValueX = hiddenValue.FindPropertyRelative("x");
						var hiddenValueY = hiddenValue.FindPropertyRelative("y");
						var hiddenValueZ = hiddenValue.FindPropertyRelative("y");

						var union = new FloatIntBytesUnion { i = hiddenValueX.intValue };
						union.b4.Shuffle();
						hiddenValueX.intValue = union.i;

						union.i = hiddenValueY.intValue;
						union.b4.Shuffle();
						hiddenValueY.intValue = union.i;

						union.i = hiddenValueZ.intValue;
						union.b4.Shuffle();
						hiddenValueZ.intValue = union.i;

						modified = true;

						break;
					}
					case "ObscuredQuaternion":
					{
						if (currentSelection == MigrationSelection.Basic) break;

						var hiddenValue = sp.FindPropertyRelative("hiddenValue");
						if (hiddenValue == null) continue;

						var hiddenValueX = hiddenValue.FindPropertyRelative("x");
						var hiddenValueY = hiddenValue.FindPropertyRelative("y");
						var hiddenValueZ = hiddenValue.FindPropertyRelative("y");
						var hiddenValueW = hiddenValue.FindPropertyRelative("w");

						var union = new FloatIntBytesUnion { i = hiddenValueX.intValue };
						union.b4.Shuffle();
						hiddenValueX.intValue = union.i;

						union.i = hiddenValueY.intValue;
						union.b4.Shuffle();
						hiddenValueY.intValue = union.i;

						union.i = hiddenValueZ.intValue;
						union.b4.Shuffle();
						hiddenValueZ.intValue = union.i;

						union.i = hiddenValueW.intValue;
						union.b4.Shuffle();
						hiddenValueW.intValue = union.i;

						modified = true;

						break;
					}
				}
			}

			return modified;
		}

		private static void EncryptAndSetBytes(string val, SerializedProperty prop, string key)
		{
			var encrypted = ObscuredString.EncryptDecrypt(val, key);
			var encryptedBytes = GetBytes(encrypted);

			prop.ClearArray();
			prop.arraySize = encryptedBytes.Length;

			for (var i = 0; i < encryptedBytes.Length; i++)
			{
				prop.GetArrayElementAtIndex(i).intValue = encryptedBytes[i];
			}
		}

		private static byte[] GetBytes(string str)
		{
			var bytes = new byte[str.Length * sizeof(char)];
			Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		private static string GetString(byte[] bytes)
		{
			var chars = new char[bytes.Length / sizeof(char)];
			Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct LongBytesUnion
		{
			[FieldOffset(0)]
			public long l;

			[FieldOffset(0)]
			public ACTkByte8 b8;
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct FloatIntBytesUnion
		{
			[FieldOffset(0)]
			public float f;

			[FieldOffset(0)]
			public int i;

			[FieldOffset(0)]
			public ACTkByte4 b4;
		}
	}
}
#endif