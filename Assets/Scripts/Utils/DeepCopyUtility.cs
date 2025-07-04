using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Utils
{
    public static class DeepCopyUtility
    {
        public static T DeepCopy<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
