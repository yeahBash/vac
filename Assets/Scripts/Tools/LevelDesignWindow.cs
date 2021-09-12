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
        private Destroyer.Destroyer _destroyer;
        private Core.Core _core;
        private Vector2? _destroyerPos;
        public List<BranchParameters> Branches = new List<BranchParameters>();
        public List<GameObject> BranchesObjs = new List<GameObject>();

        [MenuItem("Tools/Level Design")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LevelDesignWindow));
        }

        private void OnGUI()
        {
            DestroyerSettings();
            CoreSettings();
        }

        private void DestroyerSettings()
        {
            _destroyer = EditorGUILayout.ObjectField("Destroyer", _destroyer, typeof(Destroyer.Destroyer), true) as Destroyer.Destroyer;

            EditorGUILayout.BeginFadeGroup(_destroyer == null ? 0f : 1f);
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
            _core = EditorGUILayout.ObjectField("Core", _core, typeof(Core.Core), true) as Core.Core;

            EditorGUILayout.BeginFadeGroup(_core == null ? 0f : 1f);
            if (_core != null)
            {
                var so = new SerializedObject(this);
                var branchesProperty = so.FindProperty("Branches");

                EditorGUILayout.PropertyField(branchesProperty, true);
                so.ApplyModifiedProperties();

                PlaceBranches();
                Clear();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void PlaceBranches()
        {
            if (GUILayout.Button("Place"))
            {
                ClearAllBranches();
                foreach (var branch in Branches) BranchesObjs.Add(_core.PlaceBranch(branch.AnglePosition, branch.Size));
            }
        }

        private void Clear()
        {
            if (GUILayout.Button("Clear"))
                ClearAllBranches();
        }

        private void ClearAllBranches()
        {
            _core.Clear();
            foreach (var branchesObj in BranchesObjs.Where(branchesObj => branchesObj != null))
                DestroyImmediate(branchesObj);
        }
    }
}