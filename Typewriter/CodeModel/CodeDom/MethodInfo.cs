using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;

namespace Typewriter.CodeModel.CodeDom
{
    public class MethodInfo : Method
    {
        private readonly CodeFunction2 codeFunction;
        private readonly Item parent;

        private MethodInfo(CodeFunction2 codeFunction, Item parent)
        {
            this.codeFunction = codeFunction;
            this.parent = parent;
        }

        public Item Parent => parent;
        public string Name => codeFunction.Name;
        public string FullName => codeFunction.FullName;

        //public bool IsEnum => Type.IsEnum;
        //public bool IsEnumerable => Type.IsEnumerable;
        public bool IsGeneric => codeFunction.IsGeneric;
        //public bool IsNullable => Type.IsNullable;
        //public bool IsPrimitive => Type.IsPrimitive;

        private Attribute[] attributes;
        public ICollection<Attribute> Attributes => attributes ?? (attributes = AttributeInfo.FromCodeElements(codeFunction.Attributes, this).ToArray());

        private Type[] genericTypeArguments;
        public ICollection<Type> GenericTypeArguments => genericTypeArguments ?? (genericTypeArguments = GenericTypeInfo.FromFullName(codeFunction.FullName.Remove(0, parent.FullName.Length + 1), this).ToArray());

        private Parameter[] parameters;
        public ICollection<Parameter> Parameters => parameters ?? (parameters = ParameterInfo.FromCodeElements(codeFunction.Parameters, this).ToArray());

        private Type type;
        public Type Type => type ?? (type = TypeInfo.FromCodeElement(codeFunction, this));

        internal static IEnumerable<Method> FromCodeElements(CodeElements codeElements, Item parent)
        {
            return codeElements.OfType<CodeFunction2>().Where(f => f.Access == vsCMAccess.vsCMAccessPublic && f.FunctionKind != vsCMFunction.vsCMFunctionConstructor && f.IsShared == false).Select(f => new MethodInfo(f, parent));
        }
    }
}