using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vac.Tools
{
    public class LevelDesignWindow : EditorWindow
    {
        private Cutter.Cutter _cutter;
        private GameObject _virusObj;
        private Vector2? _laserPos;

        [MenuItem("Tools/Level Design")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LevelDesignWindow));
        }

        private void OnGUI()
        {
            LaserSettings();
            VirusSettings();
        }

        private void LaserSettings()
        {
            _cutter = EditorGUILayout.ObjectField("Laser", _cutter, typeof(Cutter.Cutter), true) as Cutter.Cutter;

            EditorGUILayout.BeginFadeGroup(_cutter == null ? 0f : 1f);
            if (_cutter != null)
            {
                if (!_laserPos.HasValue) _laserPos = _cutter.gameObject.transform.localPosition;
                _laserPos = EditorGUILayout.Vector2Field("Laser Position", _laserPos.Value);
                _cutter.gameObject.transform.localPosition = _laserPos.Value;
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void VirusSettings()
        {
            _virusObj = EditorGUILayout.ObjectField("Virus", _virusObj, typeof(GameObject), true) as GameObject;
        }
    }
}