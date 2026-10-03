#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Reanim2UnityAnim.Editor.Data;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Reanim2UnityAnim.Editor
{
	public static class UnitBuilder
	{
		private const float ToRad = 3.14159265f / 180;
		private static bool _useparent = false;
		public static void Create(Reanim2UnityAnimConfig config,bool pivot, bool useparent)
		{
			List<Track> tracks = TrackParser.ParseTracksFromFile(config.filePath);

			List<SpriteTrack> spriteTracks = new List<SpriteTrack>();
			List<Partition> partitions = new List<Partition>(config.customPartitions);
			List<RootTrack> rootTracks = new List<RootTrack>();
			ClassifyTracks(tracks, partitions, spriteTracks, rootTracks);

			List<AnimationClip> clips = new List<AnimationClip>();
			string name = Path.GetFileNameWithoutExtension(config.filePath);
			_useparent = useparent;
            foreach (Partition partition in partitions)
			{
				AnimationClip clip = new AnimationClip
				{ frameRate = 12, name = name + "_" + partition.name };

				CreateSpriteFrames(spriteTracks, partition, clip, config.center,pivot);
                clips.Add(clip);
            }

			CreateAssets(name, spriteTracks, clips);
		}

		private static void CreateSpriteFrames(List<SpriteTrack> spriteTracks, Partition partition, AnimationClip clip, Vector2 center,bool pivot)
		{
			foreach (SpriteTrack spriteTrack in spriteTracks)
			{
				float? x, y, az, sx, sy, a;
				x = y = az = 0;
				sx = sy = a = 1;
				int? f = 0;

				Vector2 spritehalfsize = Vector2.zero;
				if (pivot)
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Reanim2UnityAnim/reanim_all/" + spriteTrack.ImageName + ".png");
                    spritehalfsize = new Vector2(sprite.rect.width, sprite.rect.height) / sprite.pixelsPerUnit / 2;
                }


                List <Keyframe> keyframesX = new List<Keyframe>();
				List<Keyframe> keyframesY = new List<Keyframe>();
                List<Keyframe> keyframesAngleY = new List<Keyframe>();
                List<Keyframe> keyframesSx = new List<Keyframe>();
				List<Keyframe> keyframesSy = new List<Keyframe>();
				List<Keyframe> keyframesF = new List<Keyframe>();
				List<Keyframe> keyframesA = new List<Keyframe>();

				for (int frameIndex = 0; frameIndex < partition.endIndexExclude; frameIndex++)
				{
					Frame frame = spriteTrack.Transforms[frameIndex];
                    if (frameIndex <= partition.startIndexInclude)
					{
						if (frame.X != null) x = frame.X.Value / 80 + spritehalfsize.x;
						if (frame.Y != null) y = -frame.Y.Value / 80 - spritehalfsize.y;
                        if (frame.Ky != null) az = -frame.Ky.Value;
						if (frame.Sx != null) sx = frame.Sx.Value / 0.8f;
						if (frame.Sy != null) sy = frame.Sy.Value / 0.8f;
						if (frame.F != null) f = frame.F.Value + 1;
						if (frame.A != null) a = frame.A.Value;
					}
					else
					{
						x = frame.X / 80 + spritehalfsize.x;
						y = -frame.Y / 80 - spritehalfsize.y;
						az = -frame.Ky;
						sx = frame.Sx / 0.8f;
						sy = frame.Sy / 0.8f;
						f = frame.F + 1;
						a = frame.A;
					}


                    if (frameIndex < partition.startIndexInclude) continue;
					if (frameIndex >= partition.startIndexInclude)
					{
						int currentFrameInPartition = frameIndex - partition.startIndexInclude;
						float currentTime = currentFrameInPartition / 12f;

						float dx, dy;
						if (spriteTrack.Parent != null)
						{
							dx = spriteTrack.Parent.startX;
							dy = spriteTrack.Parent.startY;
						}
						else
						{
							dx = center.x;
							dy = -center.y;
                        }

                        if (x != null) keyframesX.Add(new Keyframe(currentTime, x.Value - dx));
						if (y != null) keyframesY.Add(new Keyframe(currentTime, y.Value - dy));

						if (az != null) keyframesAngleY.Add(new Keyframe(currentTime, az.Value));

                        if (sx != null) keyframesSx.Add(new Keyframe(currentTime, sx.Value));
						if (sy != null) keyframesSy.Add(new Keyframe(currentTime, sy.Value));

						if (f != null) keyframesF.Add(new Keyframe(currentTime, f.Value));
						if (a != null) keyframesA.Add(new Keyframe(currentTime, a.Value));
					}
				}

				BindKeyframes(keyframesX, clip, spriteTrack.Path, typeof(Transform), "localPosition.x");
				BindKeyframes(keyframesY, clip, spriteTrack.Path, typeof(Transform), "localPosition.y");
				BindKeyframes(keyframesAngleY, clip, spriteTrack.Path, typeof(Transform), "localEulerAngles.z");

				BindKeyframes(keyframesSx, clip, spriteTrack.Path, typeof(Transform), "localScale.x");
                BindKeyframes(keyframesSy, clip, spriteTrack.Path, typeof(Transform), "localScale.y");
                BindKeyframes(keyframesF, clip, spriteTrack.Path, typeof(GameObject), "m_IsActive");
				BindKeyframes(keyframesA, clip, spriteTrack.Path, typeof(SpriteRenderer), "m_Color.a");
			}
		}


        private static void ClassifyTracks(List<Track> tracks, List<Partition> partitions, List<SpriteTrack> spriteTracks, List<RootTrack> rootTracks)
		{
			RootTrack? currentRoot = null;
			for (int index = 0; index < tracks.Count; index++)
			{
				Track track = tracks[index];
				if (track.Name == "_ground")
				{
					RootTrack groundTrack = new RootTrack(track.Name, track.Transforms);
					rootTracks.Add(groundTrack);
					continue;
				}
				
				List<string> sprites = new List<string>();
				foreach (Frame frame in track.Transforms)
				{
					if (frame.Image != null)
					{
						sprites.Add(frame.Image);
					}
				}
				if (sprites.Count == 0)
				{
					Partition partition = new Partition(track);
					partitions.Add(partition);

					if (track.Transforms.Count(frame => frame.X != null || frame.Y != null) > 2)
					{
						currentRoot = new RootTrack(track.Name, track.Transforms);
						rootTracks.Add(currentRoot);
					}
				}

				foreach (string sprite in sprites)
				{
					if (spriteTracks.Find(spriteTrack => spriteTrack.ImageName == sprite && spriteTrack.Name == track.Name) == null)
					{
						SpriteTrack spriteTrack = new SpriteTrack(track.Name, track.Transforms, index, sprite);
						spriteTrack.Parent = currentRoot;
						spriteTracks.Add(spriteTrack);
					}
				}
			}
			IEnumerable<IGrouping<string, SpriteTrack>> groupBy = spriteTracks.GroupBy(track => track.ImageName);

			foreach (IGrouping<string, SpriteTrack> group in groupBy)
			{
				if (group.Count() == 1)
				{
					foreach (SpriteTrack spriteTrack in group)
					{
						spriteTrack.Name = spriteTrack.ImageName;
					}
				}
				else
				{
					foreach (SpriteTrack spriteTrack in group)
					{
						spriteTrack.Name = $"{spriteTrack.ImageName}({spriteTrack.Name})";
					}
				}
			}
		}

		private static void BindKeyframes(List<Keyframe> keyframes, AnimationClip clip, string relativePath, Type componentType, string propertyName)
		{
			AnimationCurve curve = new AnimationCurve(keyframes.ToArray());
            if (propertyName is "m_IsActive" or "m_Color.a")
			{
				for (int i = 0; i < curve.length; i++)
				{
					AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
					AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
				}
			}
			else
			{
				for (int i = 0; i < curve.length; i++)
				{
					curve.SmoothTangents(i, 0);
                }
            }
			if (_useparent)
            {
                clip.SetCurve($"{"Body"}/{relativePath}", componentType, propertyName, curve);
            }
			clip.SetCurve(relativePath, componentType, propertyName, curve);
		}

		private static void CreateAssets(string name, List<SpriteTrack> spriteTracks, List<AnimationClip> clips)
		{
			string targetFolder = $"Assets/Reanim2UnityAnim/Output/{name}/";
			Directory.CreateDirectory(targetFolder);

			GameObject gameObject = new GameObject(name);
			Transform parenttas = gameObject.transform;
            if (_useparent)
            {
                GameObject body = new GameObject("Body");
                body.transform.parent = gameObject.transform;
				parenttas = body.transform;
            }
            Animator animator = gameObject.AddComponent<Animator>();
			AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(targetFolder + name + "_Controller.controller");
			animator.runtimeAnimatorController = controller;
			UniqueMaterialController materialController = gameObject.AddComponent<UniqueMaterialController>();
			List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

			foreach (AnimationClip clip in clips)
			{
				AnimatorState state = controller.layers[0].stateMachine.AddState(clip.name);
				state.motion = clip;
				AssetDatabase.CreateAsset(clip, targetFolder + clip.name + ".anim");
				AssetDatabase.SaveAssets();
			}
			AssetDatabase.Refresh();

			Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Reanim2UnityAnim/DefaultShader.mat");

			foreach (SpriteTrack spriteTrack in spriteTracks)
			{
				GameObject child = new GameObject(spriteTrack.Name);
				Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Reanim2UnityAnim/reanim_all/" + spriteTrack.ImageName + ".png");
				SpriteRenderer spriteRenderer = child.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = sprite;
				spriteRenderer.material = mat;
				spriteRenderer.sortingOrder = spriteTrack.Order;
				if (spriteTrack.ParentPath != null)
				{
					Transform parent = parenttas.Find(spriteTrack.ParentPath);
					if (parent == null)
					{
						parent = new GameObject(spriteTrack.ParentPath).transform;
						parent.SetParent(parenttas);
					}
					child.transform.SetParent(parent);

                }
				else
				{
					child.transform.parent = parenttas;
                }
                Frame frame = spriteTrack.Transforms[0];
                Vector3 pos = Vector3.zero;
                if (frame.X != null) pos.x = frame.X.Value / 80;
                if (frame.Y != null) pos.y = -frame.Y.Value / 80;
                child.transform.localPosition = pos;
                spriteRenderers.Add(spriteRenderer);
			}
			materialController.spriteRenderers = spriteRenderers.ToArray();
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, targetFolder + $"{name}.prefab", InteractionMode.AutomatedAction);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}