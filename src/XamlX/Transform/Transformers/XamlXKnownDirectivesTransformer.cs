using System.Collections.Generic;
using System.Linq;
using XamlX.Ast;

namespace XamlX.Transform.Transformers
{
#if !XAMLIL_INTERNAL
    public
#endif
    class XamlXKnownDirectivesTransformer : IXamlXAstTransformer
    {
        public IXamlXAstNode Transform(XamlXAstTransformationContext context, IXamlXAstNode node)
        {
            if (node is XamlXAstObjectNode ni && ni.Type is XamlXAstXmlTypeReference type)
            {
                foreach (var d in context.Configuration.KnownDirectives)
                    if (type.XmlNamespace == d.ns && type.Name == d.name)
                    {
                        var vnodes = new List<IXamlXAstValueNode>();
                        foreach (var ch in ni.Children)
                        {
                            if(ch is IXamlXAstValueNode vn)
                                vnodes.Add(vn);
                            if (context.StrictMode)
                                throw new XamlXParseException(
                                    "Only value nodes are allowed as directive children elements", ch);
                            
                        }

                        return new XamlXAstXmlDirective(ni, type.XmlNamespace, type.Name, vnodes);
                    }

            }

            return node;
        }
    }
}
