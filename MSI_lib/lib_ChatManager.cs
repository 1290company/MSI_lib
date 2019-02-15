using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSI_lib
{
    public class lib_ChatManager
    {
        public string lib_JustDoIt(string controlname, object[] constructorParameters)
        {



            Type magicType = Type.GetType("MSI_lib.lib_" + controlname.ToString());
            ConstructorInfo magicConstructor = magicType.GetConstructor(Type.EmptyTypes);
            object magicClassObject = magicConstructor.Invoke(new object[] { });
           
            MethodInfo magicMethod = magicType.GetMethod("Init");
            object magicValue = magicMethod.Invoke(magicClassObject, constructorParameters);
            return magicValue.ToString();


            //System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();
            //string assemblyLocation = assemblyInfo.Location;
            //Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            //string dllpath = uriCodeBase.ToString();
            //Assembly assembly;
            //assembly = Assembly.LoadFrom(dllpath);
            //Type t = assembly.GetType("MSI_lib.lib_" + controlname.ToString());
            //var methodInfo = t.GetMethod("Init");
            //object result = null;
            //if (methodInfo == null) // the method doesn't exist
            //{
            //    // throw some exception

            //}
            //ParameterInfo[] parameters = methodInfo.GetParameters();
            //object[] parametersArray = new object[] { "Hello" };

            //object classInstance  = Activator.CreateInstance(t, null);
            //result = methodInfo.Invoke(classInstance, parametersArray);
            //return result.ToString();    
        }

        /// <summary>
        /// 回傳實作JOB的完整CLASS TYPE 資訊
        /// </summary>
        private IEnumerable<Type> GetAllTypesImplementingInterface(Type desiredType)
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => desiredType.IsAssignableFrom(type));

        }

        /// <summary>
        /// 檢查CLASS資訊,是否有誤
        /// </summary>
        /// <param name="testType"></param>
        /// <returns></returns>
        public static bool IsRealClass(Type testType)
        {
            return testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false;
        }

    }
}
