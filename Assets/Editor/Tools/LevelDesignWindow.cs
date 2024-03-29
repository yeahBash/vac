using System.Collections.Generic;
using System.Linq;
using Branch;
using Core;
using Destroyer;
using UnityEditor;
using UnityEngine;

namespace Editor.Tools
{
    public class LevelDesignWindow : EditorWindow
    {
        private bool _isRotateOn;
        private bool _isDestroyerShouldEdit;
        private bool _isCoreShouldEdit;
        private DestroyerBase _destroyerBase;
        private CoreBase _coreBase;
        private Vector2? _destroyerPos;
        private Vector2 _scrollPosition;
        public List<BranchBaseParameters> BranchesParameters = new List<BranchBaseParameters>();
        public List<BranchBase> Branches = new List<BranchBase>();

        private bool IsRotateOn
        {
            get => _isRotateOn;
            set
            {
                if (_coreBase == null)
                    return;

                _isRotateOn = value;
                _coreBase.IsRotateOn = _isRotateOn;
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
            Initialize(ref _destroyerBase);
            EditorGUILayout.BeginFadeGroup(_destroyerBase == null ? 0f : 1f);

            _destroyerBase = EditorGUILayout.ObjectField("Destroyer", _destroyerBase, typeof(DestroyerBase), true, GUILayout.ExpandWidth(true)) as DestroyerBase;

            if (_destroyerBase != null)
            {
                if (!_destroyerPos.HasValue) _destroyerPos = _destroyerBase.gameObject.transform.localPosition;
                _destroyerPos = EditorGUILayout.Vector2Field("Destroyer Position", _destroyerPos.Value);
                _destroyerBase.gameObject.transform.localPosition = _destroyerPos.Value;
            }

            EditorGUILayout.EndFadeGroup();
        }

        private void CoreSettings()
        {
            Initialize(ref _coreBase);

            EditorGUILayout.BeginFadeGroup(_coreBase == null ? 0f : 1f);

            _coreBase = EditorGUILayout.ObjectField("Core", _coreBase, typeof(CoreBase), true, GUILayout.ExpandWidth(true)) as CoreBase;

            if (_coreBase != null)
            {
                IsRotateOn = EditorGUILayout.Toggle("Rotate On", IsRotateOn);

                var so = new SerializedObject(this);
                var branchesParametersProperty = so.FindProperty("BranchesParameters");
                EditorGUILayout.PropertyField(branchesParametersProperty, true);
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
                Branches = _coreBase.PlaceBranches(BranchesParameters).ToList();
            }
        }

        private void Clear()
        {
            if (GUILayout.Button("Clear"))
                ClearAllBranches();
        }

        private void SizeAdjustment()
        {
            foreach (var core in Branches.Where(b => b != null))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 40f;
                core.Length = EditorGUILayout.Slider("Length", core.Length, 0f, 1.5f);
                core.AnglePosition = EditorGUILayout.Slider("Angle", core.AnglePosition, 0f, 359.9f);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void ClearAllBranches()
        {
            _coreBase.ClearBranches();
            foreach (var body in Branches.Where(branchesObj => branchesObj != null))
                Destroy(body.gameObject);
        }
    }
}