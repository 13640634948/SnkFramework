namespace SnkFramework.NuGet
{
    namespace Basic
    {
        public interface ISnkJsonParser
        {
            /// <summary>
            /// 对象转换至Json格式字符串
            /// </summary>
            /// <param name="target">序列化对象</param>
            /// <returns>Json字符串</returns>
              string ToJson(object target);

            /// <summary>
            /// 从Json字符串构建出对象
            /// </summary>
            /// <param name="json">json字符串</param>
            /// <typeparam name="T">需要构建的对象</typeparam>
            /// <returns>反序列化对象</returns>
              T FromJson<T>(string json) where T : class;
        }
    }
}

