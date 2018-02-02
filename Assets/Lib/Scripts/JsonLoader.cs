using System.IO;
using UnityEngine;

namespace MyLib
{

    public class JsonLoader : MonoBehaviour
    {

        [SerializeField, Tooltip("Relative json file path from StreamingAssets.")]
        private string m_RelativePath = "";

        private string RelativePath
        {
            get { return Application.streamingAssetsPath + "/" + m_RelativePath; }
        }


        public T Load<T> (string path = null)
        {
            // Arg check
            if (string.IsNullOrEmpty(path))
            {
                path = RelativePath;
            }

            string jsonText = File.ReadAllText(path);
            T t = JsonUtility.FromJson<T>(jsonText);

            return t;
        }

    }

}