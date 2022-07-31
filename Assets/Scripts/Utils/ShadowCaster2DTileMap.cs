using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Utils
{
    [RequireComponent(typeof(CompositeCollider2D))]
    public class ShadowCaster2DTileMap : MonoBehaviour
    {
        [SerializeField] private bool selfShadows = true;

        private CompositeCollider2D tilemapCollider;

        static readonly FieldInfo meshField =
            typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);

        static readonly FieldInfo shapePathField =
            typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);

        static readonly MethodInfo generateShadowMeshMethod = typeof(ShadowCaster2D)
            .Assembly
            .GetType("UnityEngine.Rendering.Universal.ShadowUtility")
            .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);

        [Button]
        public void Generate()
        {
            DestroyAllChildren();

            tilemapCollider = GetComponent<CompositeCollider2D>();

            for (var i = 0; i < tilemapCollider.pathCount; i++)
            {
                var pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
                tilemapCollider.GetPath(i, pathVertices);
                GameObject shadowCaster = new GameObject("shadow_caster_" + i);
                shadowCaster.transform.parent = gameObject.transform;
                ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
                shadowCasterComponent.selfShadows = this.selfShadows;

                Vector3[] testPath = new Vector3[pathVertices.Length];
                for (int j = 0; j < pathVertices.Length; j++)
                {
                    testPath[j] = pathVertices[j];
                }

                shapePathField.SetValue(shadowCasterComponent, testPath);
                meshField.SetValue(shadowCasterComponent, new Mesh());
                generateShadowMeshMethod.Invoke(shadowCasterComponent,
                    new object[]
                        { meshField.GetValue(shadowCasterComponent), shapePathField.GetValue(shadowCasterComponent) });
            }
        }

        public void DestroyAllChildren()
        {
            var tempList = transform.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}