using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vac.Branch;
using Vac.Core;
using Vac.Destroyer;

namespace Vac.Tools
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
        public List<BranchParameters> Branches = new List<BranchParameters>();
        public List<BranchBodyBase> Bodies = new List<BranchBodyBase>();

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

            _destroyerBase = EditorGUILayout.ObjectField("Destroyer", _destroyerBase, typeof(Destroyer.DestroyerBase), true, GUILayout.ExpandWidth(true)) as Destroyer.DestroyerBase;

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

            _coreBase = EditorGUILayout.ObjectField("Core", _coreBase, typeof(Core.CoreBase), true, GUILayout.ExpandWidth(true)) as Core.CoreBase;

            if (_coreBase != null)
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
                foreach (var branch in Branches) Bodies.Add(_coreBase.PlaceBranch(branch.AnglePosition, branch.Size));
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
                body.Size = EditorGUILayout.Slider("Size", body.Size, 0f, 5f);
                body.AnglePosition = EditorGUILayout.Slider("Angle", body.AnglePosition, 0f, 360f);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void ClearAllBranches()
        {
            _coreBase.Clear();
            foreach (var body in Bodies.Where(branchesObj => branchesObj != null))
                Destroy(body.gameObject);
        }
    }
}