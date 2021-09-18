using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vac.Branch;

namespace Vac.Tools
{
    public class LevelDesignWindow : EditorWindow
    {
        private bool _isRotateOn;
        private bool _isDestroyerShouldEdit;
        private bool _isCoreShouldEdit;
        private Destroyer.Destroyer _destroyer;
        private Core.Core _core;
        private Vector2? _destroyerPos;
        private Vector2 _scrollPosition;
        public List<BranchParameters> Branches = new List<BranchParameters>();
        public List<Body> Bodies = new List<Body>();

        private bool IsRotateOn
        {
            get => _isRotateOn;
            set
            {
                if (_core == null)
                    return;

                _isRotateOn = value;
                _core.IsRotateOn = _isRotateOn;
            }
        }

        [MenuItem("Tools/Level Design")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LevelDesignWindow));
        }

        private void OnGUI()
        {
            #if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                _isDestroyerShouldEdit = EditorGUILayout.Toggle("Edit Destroyer", _isDestroyerShouldEdit);
                _isCoreShouldEdit = EditorGUILayout.Toggle("Edit Core", _isCoreShouldEdit);

                if (_isDestroyerShouldEdit) DestroyerSettings();
                if (_isCoreShouldEdit) CoreSettings();

                EditorGUILayout.EndScrollView();
            } else
            {
                EditorGUILayout.LabelField("Level Design Tool should run in Play mode.");
            }
            #endif
        }

        private void Initialize<T>(ref T obj) where T : MonoBehaviour
        {
            if (obj == null)
            {
                EditorGUILayout.LabelField($"No {typeof(T)} in the scene");
                obj = FindObjectOfType<T>();
            }
        }

        private void DestroyerSettings()
        {
            Initialize(ref _destroyer);
            EditorGUILayout.BeginFadeGroup(_destroyer == null ? 0f : 1f);

            _destroyer = EditorGUILayout.ObjectField("Destroyer", _destroyer, typeof(Destroyer.Destroyer), true, GUILayout.ExpandWidth(true)) as Destroyer.Destroyer;

            if (_destroyer != null)
            {
                if (!_destroyerPos.HasValue) _destroyerPos = _destroyer.gameObject.transform.localPosition;
                _destroyerPos = EditorGUILayout.Vector2Field("Destroyer Position", _destroyerPos.Value);
                _destroyer.gameObject.transform.localPosition = _destroyerPos.Value;
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void CoreSettings()
        {
            Initialize(ref _core);
            EditorGUILayout.BeginFadeGroup(_core == null ? 0f : 1f);

            _core = EditorGUILayout.ObjectField("Core", _core, typeof(Core.Core), true, GUILayout.ExpandWidth(true)) as Core.Core;

            if (_core != null)
            {
                IsRotateOn = EditorGUILayout.Toggle("Rotate On", IsRotateOn);

                var so = new SerializedObject(this);
                var branchesProperty = so.FindProperty("Branches");
                EditorGUILayout.PropertyField(branchesProperty, true);
                so.ApplyModifiedProperties();

                PlaceBranches();
                SizeAdjustment();
                Clear();
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void PlaceBranches()
        {
            if (GUILayout.Button("Place"))
            {
                ClearAllBranches();
                foreach (var branch in Branches) Bodies.Add(_core.PlaceBranch(branch.AnglePosition, branch.Size));
            }
        }

        private void Clear()
        {
            if (GUILayout.Button("Clear"))
                ClearAllBranches();
        }

        private void SizeAdjustment()
        {
            foreach (var body in Bodies.Where(b => b != null))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 40f;
                body.Size = EditorGUILayout.Slider("Size", body.Size, 0f, 2f);
                body.AnglePosition = EditorGUILayout.Slider("Angle", body.AnglePosition, 0f, 360f);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void ClearAllBranches()
        {
            _core.Clear();
            foreach (var body in Bodies.Where(branchesObj => branchesObj != null))
                Destroy(body.gameObject);
        }
    }
}