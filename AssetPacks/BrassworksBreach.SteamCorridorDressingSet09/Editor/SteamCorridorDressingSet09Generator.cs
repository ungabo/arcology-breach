using System;
using System.Collections.Generic;
using System.IO;
using BrassworksBreach.SteamCorridorDressingSet09;
using UnityEditor;
using UnityEngine;

namespace BrassworksBreach.SteamCorridorDressingSet09.Editor
{
    public static class SteamCorridorDressingSet09Generator
    {
        private const string MenuRoot = "Brassworks Breach/Sidecars/Steam Corridor Dressing Set 09/";

        [MenuItem(MenuRoot + "Generate Package Assets")]
        public static void GeneratePackageAssets()
        {
            string packageRoot = ResolvePackageRoot();
            string generatedRoot = packageRoot + "/Runtime/Generated";

            if (AssetDatabase.IsValidFolder(generatedRoot))
            {
                AssetDatabase.DeleteAsset(generatedRoot);
            }

            EnsureFolderPath(generatedRoot);
            EnsureFolderPath(generatedRoot + "/Materials");
            EnsureFolderPath(generatedRoot + "/Meshes");
            EnsureFolderPath(generatedRoot + "/Prefabs");

            Dictionary<SteamMaterialKey, Material> materials = CreateMaterials(generatedRoot + "/Materials");
            Dictionary<SteamMeshKey, Mesh> meshes = CreateMeshes(generatedRoot + "/Meshes");

            foreach (SteamPieceDefinition piece in SteamCorridorDressingSet09Catalog.Pieces)
            {
                CreatePiecePrefab(piece, materials, meshes, generatedRoot + "/Prefabs");
            }

            CreatePalettePrefab(materials, meshes, generatedRoot + "/Prefabs");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                $"Generated Steam Corridor Dressing Set 09 assets at {generatedRoot}: " +
                $"{SteamCorridorDressingSet09Catalog.Pieces.Length} piece prefabs, " +
                $"{SteamCorridorDressingSet09Catalog.Materials.Length} materials, {meshes.Count} meshes, 1 palette prefab.");
        }

        [MenuItem(MenuRoot + "Select Runtime Catalog")]
        public static void SelectRuntimeCatalog()
        {
            string packageRoot = ResolvePackageRoot();
            UnityEngine.Object catalog = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(
                packageRoot + "/Runtime/Metadata/SCD09_SteamCorridorDressingSet09_Catalog_0.1.54-p001.json");

            Selection.activeObject = catalog;
            EditorGUIUtility.PingObject(catalog);
        }

        private static Dictionary<SteamMaterialKey, Material> CreateMaterials(string folder)
        {
            Dictionary<SteamMaterialKey, Material> results = new Dictionary<SteamMaterialKey, Material>();
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            foreach (SteamMaterialDefinition definition in SteamCorridorDressingSet09Catalog.Materials)
            {
                Material material = new Material(shader)
                {
                    name = "SCD09_MAT_" + definition.Key
                };

                ApplyMaterialProperties(material, definition);
                AssetDatabase.CreateAsset(material, $"{folder}/{material.name}.mat");
                results.Add(definition.Key, material);
            }

            return results;
        }

        private static void ApplyMaterialProperties(Material material, SteamMaterialDefinition definition)
        {
            SetColor(material, "_BaseColor", definition.BaseColor);
            SetColor(material, "_Color", definition.BaseColor);
            SetFloat(material, "_Metallic", definition.Metallic);
            SetFloat(material, "_Smoothness", definition.Smoothness);
            SetFloat(material, "_Glossiness", definition.Smoothness);

            if (definition.EmissionStrength > 0f)
            {
                Color emission = definition.EmissionColor * definition.EmissionStrength;
                material.EnableKeyword("_EMISSION");
                SetColor(material, "_EmissionColor", emission);
            }
        }

        private static void SetColor(Material material, string propertyName, Color value)
        {
            if (material.HasProperty(propertyName))
            {
                material.SetColor(propertyName, value);
            }
        }

        private static void SetFloat(Material material, string propertyName, float value)
        {
            if (material.HasProperty(propertyName))
            {
                material.SetFloat(propertyName, value);
            }
        }

        private static Dictionary<SteamMeshKey, Mesh> CreateMeshes(string folder)
        {
            Dictionary<SteamMeshKey, Mesh> results = new Dictionary<SteamMeshKey, Mesh>
            {
                { SteamMeshKey.Box, CreateBoxMesh("SCD09_MESH_Box") },
                { SteamMeshKey.Cylinder16, CreateCylinderMesh("SCD09_MESH_Cylinder16", 16) },
                { SteamMeshKey.Cylinder24, CreateCylinderMesh("SCD09_MESH_Cylinder24", 24) },
                { SteamMeshKey.Cylinder32, CreateCylinderMesh("SCD09_MESH_Cylinder32", 32) },
                { SteamMeshKey.Torus24, CreateTorusMesh("SCD09_MESH_Torus24", 24, 8) },
                { SteamMeshKey.Torus32, CreateTorusMesh("SCD09_MESH_Torus32", 32, 10) }
            };

            foreach (KeyValuePair<SteamMeshKey, Mesh> pair in results)
            {
                AssetDatabase.CreateAsset(pair.Value, $"{folder}/SCD09_MESH_{pair.Key}.asset");
            }

            return results;
        }

        private static void CreatePiecePrefab(
            SteamPieceDefinition piece,
            Dictionary<SteamMaterialKey, Material> materials,
            Dictionary<SteamMeshKey, Mesh> meshes,
            string prefabFolder)
        {
            GameObject root = new GameObject(SteamCorridorDressingSet09Catalog.GetPrefabName(piece));
            try
            {
                BuildPieceChildren(piece, root.transform, materials, meshes);
                string path = $"{prefabFolder}/{root.name}.prefab";
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, path, out bool success);
                if (!success || prefab == null)
                {
                    throw new InvalidOperationException("Could not save prefab at " + path);
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void CreatePalettePrefab(
            Dictionary<SteamMaterialKey, Material> materials,
            Dictionary<SteamMeshKey, Mesh> meshes,
            string prefabFolder)
        {
            GameObject root = new GameObject("SCD09_PREFAB_000_FullCorridorDressingPalette");
            try
            {
                Dictionary<SteamDressingFamily, int> familyRows = new Dictionary<SteamDressingFamily, int>();
                foreach (SteamPieceDefinition piece in SteamCorridorDressingSet09Catalog.Pieces)
                {
                    if (!familyRows.ContainsKey(piece.Family))
                    {
                        familyRows.Add(piece.Family, 0);
                    }

                    GameObject pieceRoot = new GameObject(SteamCorridorDressingSet09Catalog.GetPrefabName(piece));
                    pieceRoot.transform.SetParent(root.transform, false);

                    int column = (int)piece.Family;
                    int row = familyRows[piece.Family]++;
                    pieceRoot.transform.localPosition = new Vector3(column * 4.75f, 0f, row * 3.35f);
                    BuildPieceChildren(piece, pieceRoot.transform, materials, meshes);
                }

                string path = $"{prefabFolder}/{root.name}.prefab";
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, path, out bool success);
                if (!success || prefab == null)
                {
                    throw new InvalidOperationException("Could not save palette prefab at " + path);
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void BuildPieceChildren(
            SteamPieceDefinition piece,
            Transform parent,
            Dictionary<SteamMaterialKey, Material> materials,
            Dictionary<SteamMeshKey, Mesh> meshes)
        {
            foreach (SteamPartDefinition part in piece.Parts)
            {
                GameObject child = new GameObject(part.Name);
                child.transform.SetParent(parent, false);
                child.transform.localPosition = part.LocalPosition;
                child.transform.localEulerAngles = part.LocalEulerAngles;
                child.transform.localScale = part.LocalScale;

                MeshFilter filter = child.AddComponent<MeshFilter>();
                filter.sharedMesh = meshes[part.MeshKey];

                MeshRenderer renderer = child.AddComponent<MeshRenderer>();
                renderer.sharedMaterial = materials[part.MaterialKey];
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                renderer.receiveShadows = true;
            }
        }

        private static Mesh CreateBoxMesh(string name)
        {
            Vector3[] vertices =
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f)
            };

            int[] triangles =
            {
                0, 2, 1, 0, 3, 2,
                4, 5, 6, 4, 6, 7,
                8, 10, 9, 8, 11, 10,
                12, 13, 14, 12, 14, 15,
                16, 18, 17, 16, 19, 18,
                20, 21, 22, 20, 22, 23
            };

            Mesh mesh = new Mesh
            {
                name = name,
                vertices = vertices,
                triangles = triangles
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateCylinderMesh(string name, int sides)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float step = Mathf.PI * 2f / sides;

            for (int i = 0; i < sides; i++)
            {
                float angle = i * step;
                float x = Mathf.Cos(angle) * 0.5f;
                float z = Mathf.Sin(angle) * 0.5f;
                vertices.Add(new Vector3(x, -0.5f, z));
                vertices.Add(new Vector3(x, 0.5f, z));
            }

            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                int bottom = i * 2;
                int top = bottom + 1;
                int nextBottom = next * 2;
                int nextTop = nextBottom + 1;

                triangles.Add(bottom);
                triangles.Add(top);
                triangles.Add(nextTop);
                triangles.Add(bottom);
                triangles.Add(nextTop);
                triangles.Add(nextBottom);
            }

            int bottomCenter = vertices.Count;
            vertices.Add(new Vector3(0f, -0.5f, 0f));
            int topCenter = vertices.Count;
            vertices.Add(new Vector3(0f, 0.5f, 0f));

            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                triangles.Add(bottomCenter);
                triangles.Add(next * 2);
                triangles.Add(i * 2);

                triangles.Add(topCenter);
                triangles.Add(i * 2 + 1);
                triangles.Add(next * 2 + 1);
            }

            Mesh mesh = new Mesh
            {
                name = name,
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray()
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateTorusMesh(string name, int segments, int tubeSegments)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            float majorRadius = 0.38f;
            float minorRadius = 0.055f;

            for (int segment = 0; segment <= segments; segment++)
            {
                float u = segment / (float)segments * Mathf.PI * 2f;
                Vector3 center = new Vector3(Mathf.Cos(u) * majorRadius, Mathf.Sin(u) * majorRadius, 0f);
                Vector3 radial = center.normalized;

                for (int tube = 0; tube <= tubeSegments; tube++)
                {
                    float v = tube / (float)tubeSegments * Mathf.PI * 2f;
                    Vector3 point = center + radial * (Mathf.Cos(v) * minorRadius) + Vector3.forward * (Mathf.Sin(v) * minorRadius);
                    vertices.Add(point);
                }
            }

            int row = tubeSegments + 1;
            for (int segment = 0; segment < segments; segment++)
            {
                for (int tube = 0; tube < tubeSegments; tube++)
                {
                    int a = segment * row + tube;
                    int b = (segment + 1) * row + tube;
                    int c = (segment + 1) * row + tube + 1;
                    int d = segment * row + tube + 1;

                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(d);
                }
            }

            Mesh mesh = new Mesh
            {
                name = name,
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray()
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static string ResolvePackageRoot()
        {
            string[] candidates = AssetDatabase.FindAssets("SCD09_SteamCorridorDressingSet09_Catalog_0.1.54-p001");
            foreach (string guid in candidates)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid).Replace('\\', '/');
                const string marker = "/Runtime/Metadata/";
                int markerIndex = path.IndexOf(marker, StringComparison.Ordinal);
                if (markerIndex >= 0)
                {
                    return path.Substring(0, markerIndex);
                }
            }

            if (AssetDatabase.IsValidFolder(SteamCorridorDressingSet09Catalog.PackageRoot))
            {
                return SteamCorridorDressingSet09Catalog.PackageRoot;
            }

            string packagePath = "Packages/" + SteamCorridorDressingSet09Catalog.PackageId;
            if (AssetDatabase.IsValidFolder(packagePath))
            {
                return packagePath;
            }

            throw new DirectoryNotFoundException(
                "Could not resolve Steam Corridor Dressing Set 09 package root. " +
                "Import the package as an embedded package or keep it at " + SteamCorridorDressingSet09Catalog.PackageRoot + ".");
        }

        private static void EnsureFolderPath(string folderPath)
        {
            string normalized = folderPath.Replace('\\', '/');
            if (AssetDatabase.IsValidFolder(normalized))
            {
                return;
            }

            string[] parts = normalized.Split('/');
            string current = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }
    }
}
